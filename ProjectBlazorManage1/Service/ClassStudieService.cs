
using ClosedXML.Excel;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace ProjectBlazorManage1.Service
{
    public class ClassStudieService : IClassStudieService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BranchService> _logger;
        private readonly IWebHostEnvironment _env;

        public ClassStudieService(ApplicationDbContext context, ILogger<BranchService> logger, IWebHostEnvironment env)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _env = env;

        }
        public async Task<List<ClassStudied>> GetClassStudies()
        {
            return await _context.ClassStudieds
               .Include(c => c.learningPlans)
                .ThenInclude(t => t.teacher)
                .ThenInclude(t => t.Branch)
                .Include(c => c.Branch)
               .AsNoTracking().ToListAsync();
        }
        public async Task<ClassStudied> GetClassStudyById(int id)
        {
            return await _context.ClassStudieds
                .Include(c => c.learningPlans)
                .ThenInclude(t => t.teacher)
                .ThenInclude(t => t.Branch)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddClassStudy(ClassStudied classStudie)
        {
            if (classStudie == null)
            {
                throw new ArgumentNullException(nameof(classStudie));
            }

            try
            {
                await _context.ClassStudieds.AddAsync(classStudie);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "เกิดข้อผิดพลาดขณะเพิ่มข้อมูลใหม่");
                throw new InvalidOperationException("ไม่สามารถเพิ่มข้อมูลได้ อาจมีข้อมูลซ้ำ", ex);
            }
        }
        public async Task UpdateClassStudy(ClassStudied classStudie)
        {
            var existingclassStudie = _context.ClassStudieds.FirstOrDefault(b => b.Id == classStudie.Id);
            if (existingclassStudie != null)
            {
                existingclassStudie.Id = classStudie.Id;
                existingclassStudie.StartDate = classStudie.StartDate;
                existingclassStudie.EndDate = classStudie.EndDate;
                existingclassStudie.Status = classStudie.Status;
                existingclassStudie.ClassName = classStudie.ClassName;
                existingclassStudie.termClass = classStudie.termClass;
                existingclassStudie.Branch_Id = classStudie.Branch_Id;


                await _context.SaveChangesAsync();
            }
        }
        public async Task DeleteClassStudy(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("รหัสต้องเป็นค่าที่มากกว่า 0");
            }

            try
            {
                var delete = await _context.ClassStudieds.FindAsync(id);
                if (delete == null)
                {
                    throw new KeyNotFoundException($"ไม่พบข้อมูลด้วยรหัส {id}");
                }

                _context.ClassStudieds.Remove(delete);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"เกิดข้อผิดพลาดขณะลบข้อมูลรหัส {id}");
                throw new InvalidOperationException("ไม่สามารถลบข้อมูลได้ ข้อมูลอาจถูกใช้งานอยู่", ex);
            }
        }

        public async Task<byte[]> GenerateExcel(int Id)
        {
            var generations = await GetClassStudies();
            var selectedGeneration = generations.FirstOrDefault(g => g.Id == Id);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("รายงานตารางเรียน");

                // ตั้งค่าหัวตาราง
                worksheet.Cell(1, 1).Value = "วันที่";
                worksheet.Cell(1, 2).Value = "เวลา";
                worksheet.Cell(1, 3).Value = "หัวข้อบรรยาย";
                worksheet.Cell(1, 4).Value = "เทป";
                worksheet.Cell(1, 5).Value = "ผู้บรรยาย";
                worksheet.Cell(1, 6).Value = "เบอร์โทร";
                worksheet.Cell(1, 7).Value = "สาขา";
                worksheet.Cell(1, 8).Value = "ชื่อสาขา";

                // ทำให้หัวข้อเป็นตัวหนา
                worksheet.Row(1).Style.Font.Bold = true;

                // ดึงข้อมูลจาก Studies และใส่ลงใน Excel
                var filteredStudies = selectedGeneration?.learningPlans ?? new List<LearningPlan>();
                int row = 2;
                foreach (var study in filteredStudies)
                {
                    worksheet.Cell(row, 1).Value = study.Date?.ToString("dd/MM/yyyy") ?? "";
                    worksheet.Cell(row, 2).Value = study.Time ?? "";
                    worksheet.Cell(row, 3).Value = study.Name ?? "";
                    worksheet.Cell(row, 4).Value = study.Tape ?? "";

                    var instructor = study.teacher;
                    worksheet.Cell(row, 5).Value = instructor != null ? $"{instructor.Teacher_Name} {instructor.Teacher_LastName}" : "";
                    worksheet.Cell(row, 6).Value = instructor?.Phone_Number ?? "";

                    var branch = instructor?.Branch;
                    worksheet.Cell(row, 7).Value = branch?.Branch_Id;
                    worksheet.Cell(row, 8).Value = branch?.Branch_Name ?? "";

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
        public async Task<byte[]> GenerateClassSchedulePdf(int id)
        {
            var Learning = await GetClassStudyById(id);


            using var ms = new MemoryStream();
            var writer = new PdfWriter(ms);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            // โหลดฟอนต์จาก wwwroot/fonts
            var fontPath = Path.Combine(_env.WebRootPath, "fonts", "THSarabunNew.ttf");
            var fontBoldPath = Path.Combine(_env.WebRootPath, "fonts", "THSarabunNew-Bold.ttf");

            var font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);
            var fontBold = PdfFontFactory.CreateFont(fontBoldPath, PdfEncodings.IDENTITY_H);

            // หัวเรื่อง
            document.Add(new Paragraph($"ตารางเรียน-สอน หลักสูตรครูสมาธิ รุ่นที่ {id}")
                .SetFont(fontBold).SetFontSize(18).SetTextAlignment(TextAlignment.CENTER));
            document.Add(new Paragraph("สถาบันพลังจิตตานุภาพ สาขา 58 วัดสิริกาญจนาราม ภาคเสาร์-อาทิตย์")
                .SetFont(font).SetFontSize(14).SetTextAlignment(TextAlignment.CENTER));

            // ตาราง
            float[] columnWidths = { 1f, 1f, 1.5f, 0.3f, 1f, 0.8f, 0.3f, 1.2f };
            Table table = new Table(UnitValue.CreatePercentArray(columnWidths)).UseAllAvailableWidth();

            string[] headers = { "วันที่", "เวลา", "หัวข้อบรรยาย", "เทป", "ผู้บรรยาย", "เบอร์โทร", "สาขา", "ชื่อสาขา" };
            foreach (var header in headers)
                table.AddHeaderCell(new Cell().Add(new Paragraph(header).SetFont(fontBold).SetFontSize(16)));

            foreach (var study in Learning.learningPlans.OrderBy(p => p.Date))
            {
                if (study == null) continue;

                var instructor = study.teacher;
                var branch = instructor?.Branch;

                table.AddCell(new Paragraph(study.Date?.ToString("dd/MM/yyyy") ?? "").SetFont(font).SetFontSize(14));
                table.AddCell(new Paragraph(study.Time ?? "").SetFont(font).SetFontSize(14));
                table.AddCell(new Paragraph(study.Name ?? "").SetFont(font).SetFontSize(14));
                table.AddCell(new Paragraph(study.Tape ?? "").SetFont(font).SetFontSize(14));
                table.AddCell(new Paragraph($"{instructor?.Teacher_Name ?? ""} {instructor?.Teacher_LastName ?? ""}").SetFont(font).SetFontSize(14));
                table.AddCell(new Paragraph(instructor?.Phone_Number ?? "").SetFont(font).SetFontSize(14));
                table.AddCell(new Paragraph(branch?.Branch_Id.ToString() ?? "").SetFont(font).SetFontSize(14));
                table.AddCell(new Paragraph(branch?.Branch_Name ?? "").SetFont(font).SetFontSize(14));
            }

            document.Add(table);
            document.Close();

            return ms.ToArray();
        }
        public async Task<byte[]> GenerateClassSchedulePdfSecure(int id)
        {
            var Learning = await GetClassStudyById(id);


            using var ms = new MemoryStream();
            var writer = new PdfWriter(ms);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            // โหลดฟอนต์จาก wwwroot/fonts
            var fontPath = Path.Combine(_env.WebRootPath, "fonts", "THSarabunNew.ttf");
            var fontBoldPath = Path.Combine(_env.WebRootPath, "fonts", "THSarabunNew-Bold.ttf");

            var font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);
            var fontBold = PdfFontFactory.CreateFont(fontBoldPath, PdfEncodings.IDENTITY_H);

            // หัวเรื่อง
            document.Add(new Paragraph($"ตารางเรียน-สอน หลักสูตรครูสมาธิ รุ่นที่ {id}")
                .SetFont(fontBold).SetFontSize(18).SetTextAlignment(TextAlignment.CENTER));
            document.Add(new Paragraph("สถาบันพลังจิตตานุภาพ สาขา 58 วัดสิริกาญจนาราม ภาคเสาร์-อาทิตย์")
                .SetFont(font).SetFontSize(14).SetTextAlignment(TextAlignment.CENTER));

            // ตาราง
            float[] columnWidths = { 1f, 1f, 1.5f, 0.3f, 1f, 0.8f, 1.2f };
            Table table = new Table(UnitValue.CreatePercentArray(columnWidths)).UseAllAvailableWidth();

            string[] headers = { "วันที่", "เวลา", "หัวข้อบรรยาย", "เทป", "ผู้บรรยาย", "สาขา", "ชื่อสาขา" };
            foreach (var header in headers)
                table.AddHeaderCell(new Cell().Add(new Paragraph(header).SetFont(fontBold).SetFontSize(16)));

            foreach (var study in Learning.learningPlans.OrderBy(p => p.Date))
            {
                if (study == null) continue;

                var instructor = study.teacher;
                var branch = instructor?.Branch;

                table.AddCell(new Paragraph(study.Date?.ToString("dd/MM/yyyy") ?? "").SetFont(font).SetFontSize(14));
                table.AddCell(new Paragraph(study.Time ?? "").SetFont(font).SetFontSize(14));
                table.AddCell(new Paragraph(study.Name ?? "").SetFont(font).SetFontSize(14));
                table.AddCell(new Paragraph(study.Tape ?? "").SetFont(font).SetFontSize(14));
                table.AddCell(new Paragraph($"{instructor?.Teacher_Name ?? ""} {instructor?.Teacher_LastName ?? ""}").SetFont(font).SetFontSize(14));
                table.AddCell(new Paragraph(branch?.Branch_Id.ToString() ?? "").SetFont(font).SetFontSize(14));
                table.AddCell(new Paragraph(branch?.Branch_Name ?? "").SetFont(font).SetFontSize(14));
            }

            document.Add(table);
            document.Close();

            return ms.ToArray();
        }






    }
}
