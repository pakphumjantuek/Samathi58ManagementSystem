
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Hosting;
using ProjectBlazorManage1.Model;
using Microsoft.EntityFrameworkCore;

namespace ProjectBlazorManage1.Service
{
    public class StudentService : IStudenService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BranchService> _logger;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly string _imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Students");

        public StudentService(ApplicationDbContext context, ILogger<BranchService> logger, IWebHostEnvironment webHostEnvironment)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.webHostEnvironment = webHostEnvironment;
        }
        public async Task<List<Students>> GetStudents()
        {
            try
            {
                return await _context.students
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "เกิดข้อผิดพลาดขณะดึงข้อมูลทั้งหมด");
                throw; 
            }
        }

        public async Task<Teacher> ConvertStudentToTeacherAsync(int studentId)
        {
            var student = await GetStudentId(studentId);

            var existingTeacher = await _context.Teachers
                 .FirstOrDefaultAsync(t => t.Teacher_Name == student.FirstName && t.Teacher_LastName == student.LastName);
            
            if (existingTeacher != null)
                throw new Exception("นักเรียนคนนี้ถูกแปลงเป็นผู้สอนแล้ว");

            if (student == null)
                throw new Exception("ไม่พบนักเรียนที่เลือก");

            var teacher = new Teacher
            {
                Teacher_Name = student.FirstName,
                Teacher_LastName = student.LastName,
                Phone_Number = student.Phone_Number,
                Address = student.ContactAddress,
                Occupation = student.Occupation,
                Images = student.Images,
                Initial_Id = student.Initial_Id,
                Position_detail = "-",
                Special_Skill = "-",
                Branch_Id = null,
                JobPosition_Id = null
            };
      
                _context.Teachers.Add(teacher);
                await _context.SaveChangesAsync();
            
            return teacher;
        }

        public async Task<Students> GetStudentId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("รหัสสาขาต้องเป็นค่าที่มากกว่า 0");
            }

            try
            {
                var student = await _context.students
                    .AsNoTracking()
                    .FirstOrDefaultAsync(b => b.Id == id);

                return student ?? throw new KeyNotFoundException($"ไม่พบข้อมูลด้วยรหัส {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"เกิดข้อผิดพลาดขณะดึงข้อมูลสาขารหัส {id}");
                throw;
            }
        }
        public async Task AddStudent(Students student)
        {
            if (student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }

            try
            {
                var lastRunningId = _context.students
                .Where(x => x.ClassStudie_Id == student.ClassStudie_Id)
                .Max(x => (int?)x.ClassStudie_RunningId) ?? 0;

                student.ClassStudie_RunningId = lastRunningId + 1;
                await _context.students.AddAsync(student);
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "เกิดข้อผิดพลาดขณะเพิ่มข้อมูลใหม่");
                throw new InvalidOperationException("ไม่สามารถเพิ่มข้อมูลได้ อาจมีข้อมูลซ้ำ", ex);
            }
        }
        public async Task UpdateStudent(Students student)
        {
            var existingStudent = _context.students.FirstOrDefault(b => b.Id == student.Id);
            if (existingStudent != null)
            {

                bool isClassStudieChanged = existingStudent.ClassStudie_Id != student.ClassStudie_Id;

                existingStudent.Id = student.Id;
                existingStudent.FirstName = student.FirstName;
                existingStudent.LastName = student.LastName;
                existingStudent.Student_Id = student.Student_Id;
                existingStudent.Initial_Id = student.Initial_Id;
                existingStudent.Nickname = student.Nickname;
                existingStudent.Phone_Number = student.Phone_Number;
                existingStudent.EnglishName = student.EnglishName;
                existingStudent.BirthDate = student.BirthDate;
                existingStudent.degreeType = student.degreeType;
                existingStudent.relationshipType = student.relationshipType;
                existingStudent.Institution = student.Institution;
                existingStudent.CitizenCardAddress = student.CitizenCardAddress;
                existingStudent.ContactAddress = student.ContactAddress;
                existingStudent.Occupation = student.Occupation;
                existingStudent.Workplace = student.Workplace;
                existingStudent.ClosePersonPhone = student.ClosePersonPhone;
                existingStudent.MedicalCondition = student.MedicalCondition;
                existingStudent.Email = student.Email;
                existingStudent.LineId = student.LineId;
                existingStudent.ApplicationDate = student.ApplicationDate;
                existingStudent.Remarks = student.Remarks;
                existingStudent.adviser = student.adviser;
                existingStudent.Images = student.Images;
                existingStudent.ClassStudie_Id = student.ClassStudie_Id;
                existingStudent.ClosePersonName = student.ClosePersonName;
                existingStudent.credit = student.credit;
                existingStudent.Status_Medical = student.Status_Medical;

                if (isClassStudieChanged)
                {
                    existingStudent.ClassStudie_Id = student.ClassStudie_Id;

                    // หา RunningId สูงสุดใน Class ใหม่
                    var lastRunningIdInNewClass = await _context.students
                        .Where(x => x.ClassStudie_Id == student.ClassStudie_Id)
                        .MaxAsync(x => (int?)x.ClassStudie_RunningId) ?? 0;

                    existingStudent.ClassStudie_RunningId = lastRunningIdInNewClass + 1;
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteStudent(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("รหัสสาขาต้องเป็นค่าที่มากกว่า 0");
            }

            try
            {
                var student = await _context.students.FindAsync(id);
                if (student == null)
                {
                    throw new KeyNotFoundException($"ไม่พบสาขาด้วยรหัส {id}");
                }

                _context.students.Remove(student);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"เกิดข้อผิดพลาดขณะลบหัส {id}");
                throw new InvalidOperationException("ไม่สามารถลบได้ ข้อมูลอาจถูกใช้งานอยู่", ex);
            }
        }

        public byte[] GenerateStudentCards(List<int> selectedIds)
        {
            if (selectedIds == null || selectedIds.Count == 0)
                throw new ArgumentException("กรุณาเลือกนักเรียนอย่างน้อย 1 คน");

            var students = _context.students
                .Include(p => p.classStudied)
                .ThenInclude(p => p.Branch)
                .Include(p => p.Initial)
                .Where(s => selectedIds.Contains(s.Id))
                .ToList();

            if (students.Count == 0)
                throw new ArgumentException("ไม่พบข้อมูลนักเรียนที่เลือก");

            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 50, 50, 30, 0);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                document.AddTitle("บัตรนักศึกษา");
                document.AddCreationDate();

                BaseColor customColor = new BaseColor(112, 60, 23);
    
                string fontPath2 = Path.Combine(webHostEnvironment.WebRootPath, "fonts", "THSarabunNew-Bold.ttf");
                BaseFont thaiFont2 = BaseFont.CreateFont(fontPath2, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                Font ClassId = new Font(thaiFont2, 14, Font.NORMAL, customColor);
                Font ClassNAME = new Font(thaiFont2, 10, Font.NORMAL,customColor);
                Font ClassTerm = new Font(thaiFont2, 16, Font.NORMAL,customColor);
                Font Name = new Font(thaiFont2, 14, Font.NORMAL);
                Font PersonelId = new Font(thaiFont2, 12, Font.NORMAL,customColor);
                Font PersonelId1 = new Font(thaiFont2, 13, Font.NORMAL);
                Font Branchs = new Font(thaiFont2, 20, Font.NORMAL,customColor);
              
                string bgPath = Path.Combine(webHostEnvironment.WebRootPath, "images", "BG_TEST.png");
                Image bgImage = null;
                if (System.IO.File.Exists(bgPath))
                {
                    bgImage = Image.GetInstance(bgPath);
                }

                PdfPTable table = new PdfPTable(2);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 50f, 50f });
                table.SpacingBefore = 0f;
                table.SpacingAfter = 0f;
                table.DefaultCell.Border = Rectangle.NO_BORDER;
                table.DefaultCell.Padding = 0f;
                table.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                int count = 0;
                foreach (var student in students)
                {
                    float cardWidth = 240;
                    float cardHeight = 144;

                    PdfPCell cardCell = new PdfPCell();
                    cardCell.FixedHeight = cardHeight;
                    cardCell.MinimumHeight = cardHeight;
                    cardCell.Border = Rectangle.NO_BORDER;
                    cardCell.Padding = 0f;
                    cardCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cardCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    PdfContentByte cb = writer.DirectContent;
                    PdfTemplate template = cb.CreateTemplate(cardWidth, cardHeight);

                    if (bgImage != null)
                    {
                        bgImage.ScaleAbsolute(cardWidth, cardHeight);
                        bgImage.SetAbsolutePosition(0, 0);
                        template.AddImage(bgImage);
                    }
  
                    Phrase cardName = new Phrase(student.Initial.Initial_Name + student.FirstName + " " + student.LastName, Name);
                    ColumnText cardNames = new ColumnText(template);
                    cardNames.SetSimpleColumn(cardName, 50, cardHeight - 70, cardWidth, cardHeight - 100, 7, Element.ALIGN_CENTER);
                    cardNames.Go();

                    Phrase cardNameEng = new Phrase(student.EnglishName , Name);
                    ColumnText cardNameEngs = new ColumnText(template);
                    cardNameEngs.SetSimpleColumn(cardNameEng, 50, cardHeight - 70, cardWidth, cardHeight - 100, 20, Element.ALIGN_CENTER);
                    cardNameEngs.Go();
            
                    float photoWidth = 55;  // Reduced size
                    float photoHeight = 55; // Reduced size
                    float photoLeft = 10;
                    float photoBottom = cardHeight - 55 - 50; // ให้ top อยู่ที่ cardHeight - 50

                    Rectangle photoRect = new Rectangle(photoLeft, photoBottom, photoLeft + photoWidth, photoBottom + photoHeight);
                    template.Rectangle(photoRect.Left, photoRect.Bottom, photoRect.Width, photoRect.Height);
                    template.Stroke(); ;

                    // Add student image
                    if (!string.IsNullOrEmpty(student.Images))
                    {
                        try
                        {
                            var imagePaths = student.Images.Split(',');
                            string imagePath = Path.Combine(webHostEnvironment.WebRootPath, imagePaths[0].TrimStart('/'));
                            if (System.IO.File.Exists(imagePath))
                            {
                                Image studentImage = Image.GetInstance(imagePath);

                                studentImage.ScaleAbsolute(photoWidth, photoHeight); 

                                float x = photoRect.Left; 
                                float y = photoRect.Bottom + (photoRect.Height - studentImage.ScaledHeight) / 2;

                                studentImage.SetAbsolutePosition(x, y);
                                template.AddImage(studentImage);
                            }
                            else
                            {
                                Phrase photoText = new Phrase("ไม่มีรูป", ClassId);
                                ColumnText ctPhoto = new ColumnText(template);
                                ctPhoto.SetSimpleColumn(photoText, photoRect.Left, photoRect.Bottom, photoRect.Right, photoRect.Top, 15, Element.ALIGN_CENTER);
                                ctPhoto.Go();
                            }
                        }
                        catch (Exception)
                        {
                            Phrase photoText = new Phrase("รูป\nนักศึกษา", ClassId);
                            ColumnText ctPhoto = new ColumnText(template);
                            ctPhoto.SetSimpleColumn(photoText, photoRect.Left, photoRect.Bottom, photoRect.Right, photoRect.Top, 15, Element.ALIGN_CENTER);
                            ctPhoto.Go();
                        }
                    }
                    Image cardImage = Image.GetInstance(template);
                    cardCell.Image = cardImage;
                    table.AddCell(cardCell);

                    Phrase Namecard = new Phrase("รหัสนักศึกษา", PersonelId);
                    ColumnText Namecards = new ColumnText(template);
                    Namecards.SetSimpleColumn(Namecard, 10, -5, 70, 35, 15, Element.ALIGN_CENTER); // ปรับ y จาก (10,50) → (5,45)
                    Namecards.Go();

                    Phrase NameClasscard = new Phrase(student.classStudied.ClassName, ClassNAME);
                    ColumnText NameClasscards = new ColumnText(template);
                    NameClasscards.SetSimpleColumn(NameClasscard, 71, -5, 230, 45, 15, Element.ALIGN_CENTER);
                    NameClasscards.Go();

                    Phrase TermClasscard = new Phrase(student.classStudied.termClass, ClassTerm);
                    ColumnText TermClasscards = new ColumnText(template);
                    TermClasscards.SetSimpleColumn(TermClasscard, 100, -5, 200, 30, 15, Element.ALIGN_CENTER);
                    TermClasscards.Go();

                    if (student.PersonalId != null)
                    {
                        Phrase cardId = new Phrase(ConvertToThaiNumbers(student.PersonalId), PersonelId1);
                        ColumnText cardIds = new ColumnText(template);
                        cardIds.SetSimpleColumn(cardId, 10, -5, 70, 24, 15, Element.ALIGN_CENTER); // ปรับ y จาก (10,50) → (5,45)
                        cardIds.Go();
                    }
                  

                    Phrase cardClassId = new Phrase(ConvertToThaiNumbers(student.ClassStudie_Id.ToString()), ClassId);
                    ColumnText cardClassIds = new ColumnText(template);
                    cardClassIds.SetSimpleColumn(cardClassId, 183, 12, 243, 57, 15, Element.ALIGN_CENTER);
                    cardClassIds.Go();

                    Phrase cardBranch = new Phrase(ConvertToThaiNumbers(student.classStudied.Branch.Branch_Id.ToString()), Branchs);
                    ColumnText cardBranchs = new ColumnText(template);
                    cardBranchs.SetSimpleColumn(cardBranch, 180, 12, 243, 142, 15, Element.ALIGN_CENTER);
                    cardBranchs.Go();

                    Phrase cardBranchName = new Phrase(student.classStudied.Branch.Branch_Name, ClassId);
                    ColumnText cardBranchNames = new ColumnText(template);
                    cardBranchNames.SetSimpleColumn(cardBranchName, 10, 12, 243, 125, 15, Element.ALIGN_CENTER);
                    cardBranchNames.Go();

                    count++;
                    if (count % 10 == 0) // 6 cards per page
                    {
                        document.Add(table);
                        document.NewPage();
                        table.DeleteBodyRows();
                    }

                }
                if (count > 0)
                {

                    if (count % 2 != 0)
                    {
                        PdfPCell emptyCell = new PdfPCell();
                        emptyCell.FixedHeight = 144;
                        emptyCell.Border = Rectangle.NO_BORDER;
                        table.AddCell(emptyCell);
                    }
                    document.Add(table);
                }

                document.Close();
                writer.Close();

                return ms.ToArray();
            }
        }

        public static string ConvertToThaiNumbers(string input)
        {
            return input.Replace('0', '๐')
                        .Replace('1', '๑')
                        .Replace('2', '๒')
                        .Replace('3', '๓')
                        .Replace('4', '๔')
                        .Replace('5', '๕')
                        .Replace('6', '๖')
                        .Replace('7', '๗')
                        .Replace('8', '๘')
                        .Replace('9', '๙');
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
