
using ClosedXML.Excel;
using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace ProjectBlazorManage1.Service
{
    public class TeacherService : ITeacherService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TeacherService> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly string _imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Teacher");

        public TeacherService(
            ApplicationDbContext context,
            ILogger<TeacherService> logger,
            IWebHostEnvironment environment)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        public async Task<List<Teacher>> GetAllTeachers()
        {
            return await _context.Teachers
                 .Include(t => t.Initial)
                    .Include(t => t.Branch)
                    .Include(t => t.jobPosition)
                    .Include(t => t.EducationHistories)
                    .Include(t => t.DhamaHistories)
                    .ThenInclude(h => h.CourseName)
                .AsNoTracking().ToListAsync();
        }

        public async Task<List<CourseName>> GetAllCourses()
        {
            return await _context.CourseNames
                .Where(c => c.Status == true) // แสดงเฉพาะหลักสูตรที่เปิดใช้งาน
                .ToListAsync();
        }

        public async Task<Teacher> GetTeacherById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("รหัสอาจารย์ต้องเป็นค่าที่มากกว่า 0");
            }

            try
            {
                var branch = await _context.Teachers
                    .AsNoTracking()
                    .Include(t => t.Initial)
                    .Include(t => t.Branch)
                    .Include(t => t.jobPosition)
                    .Include(t => t.EducationHistories)
                    .Include(t => t.DhamaHistories)
                    .ThenInclude(t => t.CourseName)
                    .FirstOrDefaultAsync(b => b.Teacher_Id == id);

                return branch ?? throw new KeyNotFoundException($"ไม่พบอาจารย์ด้วยรหัส {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"เกิดข้อผิดพลาดขณะดึงข้อมูลอาจารย์รหัส {id}");
                throw;
            }
        }
        public async Task AddTeacher(Teacher teacher)
        {
            if (teacher == null)
            {
                throw new ArgumentNullException(nameof(teacher));
            }

            try
            {
                await _context.Teachers.AddAsync(teacher);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "เกิดข้อผิดพลาดขณะเพิ่มอาจารย์ใหม่");
                throw new InvalidOperationException("ไม่สามารถเพิ่มอาจารย์ได้ อาจมีข้อมูลซ้ำ", ex);
            }
        }
        public async Task UpdateTeacher(Teacher teacher)
        {
            var existingTeacher = _context.Teachers.FirstOrDefault(b => b.Teacher_Id == teacher.Teacher_Id);
            if (existingTeacher != null)
            {
                existingTeacher.Teacher_Name = teacher.Teacher_Name;
                existingTeacher.Teacher_LastName = teacher.Teacher_LastName;
                existingTeacher.Phone_Number = teacher.Phone_Number;
                existingTeacher.Address = teacher.Address;
                existingTeacher.Images = teacher.Images;
                existingTeacher.Occupation = teacher.Occupation;
                existingTeacher.Initial_Id = teacher.Initial_Id;
                existingTeacher.Branch_Id = teacher.Branch_Id;
                existingTeacher.JobPosition_Id = teacher.JobPosition_Id;
                existingTeacher.Position_detail = teacher.Position_detail;
                existingTeacher.Special_Skill = teacher.Special_Skill;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteTeacher(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("รหัสอาจารย์ต้องเป็นค่าที่มากกว่า 0");
            }

            try
            {
                var teacher = await _context.Teachers.FindAsync(id);
                if (teacher == null)
                {
                    throw new KeyNotFoundException($"ไม่พบอาจารย์ด้วยรหัส {id}");
                }

                _context.Teachers.Remove(teacher);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"เกิดข้อผิดพลาดขณะลบอาจารย์รหัส {id}");
                throw new InvalidOperationException("ไม่สามารถลบอาจารย์ได้ ข้อมูลอาจถูกใช้งานอยู่", ex);
            }
        }

        public async Task<byte[]> GenerateCourse()
        {
            var Course1 = await GetAllTeachers();

            using var ms = new MemoryStream();
            var writer = new PdfWriter(ms);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            // โหลดฟอนต์
            var fontPath = Path.Combine(_environment.WebRootPath, "fonts", "THSarabunNew.ttf");
            var fontBoldPath = Path.Combine(_environment.WebRootPath, "fonts", "THSarabunNew-Bold.ttf");

            var font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);
            var fontBold = PdfFontFactory.CreateFont(fontBoldPath, PdfEncodings.IDENTITY_H);

            // หัวเรื่อง
            document.Add(new Paragraph("รายชื่อผู้เข้าอบรมหลักสูตร")
                .SetFont(fontBold)
                .SetFontSize(18)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginBottom(20));

            // จัดกลุ่มตามชื่อหลักสูตรที่ Status == true
            var groupedByCourse = Course1
                .SelectMany(t => t.DhamaHistories
                    .Where(d => d.CourseName.Status == true)
                    .OrderBy(p => p.CourseName.Level)
                    .Select(d => new { CourseName = d.CourseName.Name, Teacher = t }))
                .GroupBy(x => x.CourseName);

            foreach (var group in groupedByCourse)
            {
                // ชื่อหลักสูตร = หัวเรื่อง
                document.Add(new Paragraph($"หลักสูตร: {group.Key}")
                    .SetFont(fontBold)
                    .SetFontSize(16)
                    .SetMarginTop(10)
                    .SetMarginBottom(0));

                float[] colWidths = { 1f, 2f };
                Table table = new Table(UnitValue.CreatePercentArray(colWidths)).UseAllAvailableWidth();

                table.AddHeaderCell(new Cell().Add(new Paragraph("ชื่อ-นามสกุล").SetFont(fontBold).SetFontSize(14)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("เบอร์โทรศัพท์").SetFont(fontBold).SetFontSize(14)));

                foreach (var item in group.OrderBy(p => p.Teacher.Teacher_Name))
                {
                    var teacher = item.Teacher;
                    string fullName = $"{teacher.Initial.Initial_Name} {teacher.Teacher_Name} {teacher.Teacher_LastName ?? ""}";
                    string phoneNumber = $"{teacher.Phone_Number}";

                   


                    table.AddCell(new Cell().Add(new Paragraph(fullName).SetFont(font).SetFontSize(13)));
                    table.AddCell(new Cell().Add(new Paragraph(phoneNumber).SetFont(font).SetFontSize(13)));
                }
               

                document.Add(table);
            }

            document.Close();
            return ms.ToArray();
        }
        public async Task<byte[]> GenerateCoursePdfByCourseId(int courseId)
        {
            var teachers = await _context.Teachers
                .Include(t => t.Initial)
                .Include(t => t.Branch)
                .Include(t => t.DhamaHistories)
                    .ThenInclude(h => h.CourseName)
                .Where(t => t.DhamaHistories.Any(d => d.CourseName_Id == courseId && d.CourseName.Status == true))
                .AsNoTracking()
                .ToListAsync();

            var courseName = await _context.CourseNames
                .Where(c => c.Id == courseId)
                .Select(c => c.Name)
                .FirstOrDefaultAsync();

            using var ms = new MemoryStream();
            var writer = new PdfWriter(ms);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            var fontPath = Path.Combine(_environment.WebRootPath, "fonts", "THSarabunNew.ttf");
            var fontBoldPath = Path.Combine(_environment.WebRootPath, "fonts", "THSarabunNew-Bold.ttf");
            var fontBold = PdfFontFactory.CreateFont(fontBoldPath, PdfEncodings.IDENTITY_H);
            var font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);

            document.Add(new Paragraph($"รายชื่อผู้เข้าอบรมหลักสูตร: {courseName}")
                .SetFont(font)
                .SetFontSize(18)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginBottom(20));

            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 1f, 2f })).UseAllAvailableWidth();
            table.AddHeaderCell(new Paragraph("ชื่อ-นามสกุล").SetFont(fontBold).SetFontSize(14));
            table.AddHeaderCell(new Paragraph("เบอร์โทรศัพท์").SetFont(fontBold).SetFontSize(14));

            foreach (var teacher in teachers.OrderBy(t => t.Teacher_Name))
            {
                string phoneNumber = teacher.Phone_Number ;
                string fullName = $"{teacher.Initial.Initial_Name} {teacher.Teacher_Name} {teacher.Teacher_LastName}";


                table.AddCell(new Paragraph(fullName).SetFont(font).SetFontSize(13));
                table.AddCell(new Paragraph(phoneNumber).SetFont(font).SetFontSize(13));
            }
             var count = teachers.Count();
            table.AddFooterCell(new Cell(1, 2)
                .Add(new Paragraph($"จำนวนผู้เข้าอบรมทั้งหมด: {count}").SetFont(font).SetFontSize(14))
                .SetTextAlignment(TextAlignment.LEFT)
                .SetBorder(Border.NO_BORDER));

            document.Add(table);
            document.Close();
            return ms.ToArray();
        }
        public async Task<byte[]> ExportToPdfSecurity(int id)
        {
            var teacher = await GetTeacherById(id);

            if (teacher == null)
            {
                return null;
            }

            try
            {
                using (var ms = new MemoryStream())
                {
                    var writer = new PdfWriter(ms);
                    var pdf = new PdfDocument(writer);
                    var document = new Document(pdf, iText.Kernel.Geom.PageSize.A4);
                    document.SetMargins(25, 25, 25, 25);

                    // Load fonts
                    string fontPath = Path.Combine(_environment.WebRootPath, "fonts", "cordia_new.ttf");
                    string fontBoldPath = Path.Combine(_environment.WebRootPath, "fonts", "cordia_newBold.ttf");

                    PdfFont font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);
                    PdfFont fontBold = PdfFontFactory.CreateFont(fontBoldPath, PdfEncodings.IDENTITY_H);

                    Paragraph title = new Paragraph("ประวัติอาจารย์ผู้บรรยาย")
                        .SetFont(fontBold)
                        .SetFontSize(26)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetMarginBottom(10);
                    document.Add(title);

                    Table table = new Table(2).UseAllAvailableWidth();
                    Cell textCell = new Cell().SetBorder(Border.NO_BORDER);

                    textCell.Add(new Paragraph($"ชื่อ-นามสกุล: {teacher.Teacher_Name} {teacher.Teacher_LastName}")
                        .SetFont(font)
                        .SetFontSize(24)
                        .SetMarginBottom(2));

                    textCell.Add(new Paragraph($"สังกัดสาขา: {teacher.Branch_Id + " " + teacher.Branch?.Branch_Name ?? "-"}")
                        .SetFont(font)
                        .SetFontSize(24)
                        .SetMarginBottom(0));

                    table.AddCell(textCell);

                    Cell imageCell = new Cell().SetBorder(Border.NO_BORDER)
                        .SetHorizontalAlignment(HorizontalAlignment.RIGHT);

                    if (!string.IsNullOrEmpty(teacher.Images))
                    {
                        var imagePaths = teacher.Images.Split(',');
                        string fullImagePath = Path.Combine(_environment.WebRootPath, imagePaths[0].TrimStart('/'));
                        if (File.Exists(fullImagePath))
                        {
                            ImageData imageData = ImageDataFactory.Create(fullImagePath);
                            Image image = new Image(imageData)
                                .SetWidth(120)
                                .SetHeight(120)
                                .SetHorizontalAlignment(HorizontalAlignment.RIGHT)
                                .SetBorderRadius(new BorderRadius(10));
                            imageCell.Add(image);
                        }
                    }

                    table.AddCell(imageCell);
                    document.Add(table);

                    // Dhama Histories
                    if (teacher.DhamaHistories != null && teacher.DhamaHistories.Any())
                    {
                        Paragraph courseParagraph = new Paragraph()
                            .SetFont(font)
                            .SetFontSize(24)
                            .SetTextAlignment(TextAlignment.LEFT)
                        .SetMarginTop(0)
                            .SetMarginBottom(0);

                        courseParagraph.AddTabStops(new TabStop(97f, TabAlignment.LEFT));
                        courseParagraph.Add(new Text("จบหลักสูตร  ").SetFont(fontBold));

                        bool first = true;
                        foreach (var course in teacher.DhamaHistories.OrderBy(p => p.CourseName.Level))
                        {
                            if (!first)
                            {
                                courseParagraph.Add("\n");
                                courseParagraph.Add(new Tab());
                                courseParagraph.SetMultipliedLeading(0.9f);
                            }
                            courseParagraph.Add(new Text($": {course.CourseName?.Name ?? ""} รุ่นที่: {course.generation.ToString() ?? ""}"));
                            first = false;
                        }
                        document.Add(courseParagraph);
                    }
                    else
                    {
                        document.Add(new Paragraph("ไม่มีข้อมูลประวัติทางธรรม")
                            .SetFont(font)
                            .SetFontSize(24)
                            .SetTextAlignment(TextAlignment.LEFT)
                            .SetMarginTop(0)
                            .SetMarginBottom(0));
                    }

                    // Education Histories
                    if (teacher.EducationHistories != null && teacher.EducationHistories.Any())
                    {
                        Paragraph eduHeader = new Paragraph("จบการศึกษา : ")
                            .SetFont(fontBold)
                            .SetFontSize(24)
                            .SetTextAlignment(TextAlignment.LEFT)
                        .SetMarginBottom(2);

                        foreach (var edu in teacher.EducationHistories.OrderByDescending(p => p.Degree).Take(1))
                        {
                            eduHeader.Add(new Text($"{edu.Degree.ToString() ?? ""}").SetFont(font).SetFontSize(24));
                        }
                        document.Add(eduHeader);
                    }
                    else
                    {
                        document.Add(new Paragraph("ไม่มีข้อมูลประวัติการศึกษา")
                            .SetFont(font)
                            .SetFontSize(24)
                            .SetTextAlignment(TextAlignment.LEFT)
                            .SetMarginBottom(2));
                    }

                    document.Add(new Paragraph($"อาชีพปัจจุบัน: {teacher.Occupation ?? "-"}")
                        .SetFont(font)
                        .SetFontSize(24)
                        .SetMarginBottom(2));

                    document.Add(new Paragraph($"หน้าที่ในสถาบัน: {teacher.jobPosition?.Position_Name ?? "-"}")
                        .SetFont(font)
                        .SetFontSize(24)
                        .SetMarginBottom(2));

                    document.Add(new Paragraph($"ความสารถพิเศษ: {teacher.Special_Skill ?? "-"}")
                        .SetFont(font)
                        .SetFontSize(24)
                        .SetMarginBottom(2));

                    document.Close();
                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error generating PDF for teacher {id}");
                throw;
            }
        }
        public async Task<byte[]> GenerateExcel(int courseId)
        {
          
            var teachers = await _context.Teachers
                .Include(t => t.Initial)
                .Include(t => t.Branch)
                .Include(t => t.DhamaHistories)
                    .ThenInclude(h => h.CourseName)
                .Where(t => t.DhamaHistories.Any(d => d.CourseName_Id == courseId && d.CourseName.Status == true))
                .AsNoTracking()
                .ToListAsync();

            var courseName = await _context.CourseNames
                .Where(c => c.Id == courseId)
                .Select(c => c.Name)
                .FirstOrDefaultAsync();

          
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("รายงานตารางเรียน");

                // ตั้งค่าหัวตาราง
                worksheet.Cell(1, 1).Value = "ชื่อ-นามสกุล";
                worksheet.Cell(1, 2).Value = "เบอร์โทร";
             

                // ทำให้หัวข้อเป็นตัวหนา
                worksheet.Row(1).Style.Font.Bold = true;

                // ดึงข้อมูลจาก Studies และใส่ลงใน Excel
                var filteredStudies = teachers ?? new List<Teacher>();
                int row = 2;
                foreach (var study in filteredStudies)
                {
                    worksheet.Cell(row, 1).Value =
                    (study.Initial.Initial_Name ?? "") + " " +
                    (study.Teacher_Name ?? "") + " " +
                    (study.Teacher_LastName ?? "");
                    worksheet.Cell(row, 2).Value = study.Phone_Number ?? "";

                  

                    row++;
                }

                // ปรับขนาดคอลัมน์อัตโนมัติ
                worksheet.Columns().AdjustToContents();

                // บันทึกไฟล์ Excel ลงใน MemoryStream
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }
        public async Task DeleteImage(string imageUrl)
        {
            try
            {
                var fileName = Path.GetFileName(imageUrl);
                var filePath = Path.Combine(_imagePath, fileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete image: {ex.Message}");
            }
        }

    }
}


    
