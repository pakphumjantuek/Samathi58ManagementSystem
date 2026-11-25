
using System.Text.RegularExpressions;
using System.Text;
using ExcelDataReader;
using Microsoft.EntityFrameworkCore;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout;

namespace ProjectBlazorManage1.Service
{
    public class LearningPlanService : ILearningPlanService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BranchService> _logger;
        private readonly IWebHostEnvironment _env;

        public LearningPlanService(ApplicationDbContext context, ILogger<BranchService> logger,IWebHostEnvironment env)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _env = env;
        }

        public async Task<List<LearningPlan>> GetAllAsync()
        {
            return await _context.LearningPlans
               .Include(p => p.teacher)
               .Include(p => p.teacher.Branch)
               .Include(c => c.ClassStudied)
               .ToListAsync();
        }
        public async Task<LearningPlan> GetByIdAsync(int id)
        {
            return await _context.LearningPlans.FindAsync(id);

        }

        public async Task<IEnumerable<LearningPlan>> GetLearningPlansByClassStudiedIdAsync(int classStudiedId)
        {
            return await _context.LearningPlans
                .Include(lp => lp.teacher) 
                .ThenInclude(t => t.Branch) 
                .Where(lp => lp.ClassStudiedId == classStudiedId)
                .ToListAsync();
        }
        public async Task AddLearningPlan(LearningPlan learningPlan)
        {
            if (learningPlan == null)
            {
                throw new ArgumentNullException(nameof(learningPlan));
            }

            try
            {
                await _context.LearningPlans.AddAsync(learningPlan);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "เกิดข้อผิดพลาดขณะเพิ่มข้อมูลใหม่");
                throw new InvalidOperationException("ไม่สามารถเพิ่มข้อมูลได้ อาจมีข้อมูลซ้ำ", ex);
            }
        }
       public async Task UpdateLearningPlan(LearningPlan learningPlan)
{
    var existingclearningPlan = _context.LearningPlans.FirstOrDefault(b => b.Id == learningPlan.Id);
    if (existingclearningPlan != null)
    {
        existingclearningPlan.Date = learningPlan.Date;
        existingclearningPlan.Time = learningPlan.Time;
        existingclearningPlan.Name = learningPlan.Name;
        existingclearningPlan.Tape = learningPlan.Tape;
        existingclearningPlan.Lecturer = learningPlan.Lecturer;
        existingclearningPlan.Book = learningPlan.Book;
        existingclearningPlan.Score = learningPlan.Score;
        existingclearningPlan.Status = learningPlan.Status;
        existingclearningPlan.Teacher_ID = learningPlan.Teacher_ID;
        existingclearningPlan.ClassStudiedId = learningPlan.ClassStudiedId;

        await _context.SaveChangesAsync();
    }
}

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("รหัสต้องเป็นค่าที่มากกว่า 0");
            }

            try
            {
                var delete = await _context.LearningPlans.FindAsync(id);
                if (delete == null)
                {
                    throw new KeyNotFoundException($"ไม่พบข้อมูลด้วยรหัส {id}");
                }

                _context.LearningPlans.Remove(delete);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"เกิดข้อผิดพลาดขณะข้อมูลรหัส {id}");
                throw new InvalidOperationException("ไม่สามารถลบข้อมูลได้ ข้อมูลอาจถูกใช้งานอยู่", ex);
            }
        }

        public async Task<string> UploadExcelAsync(Stream stream, int classStudiedId)
        {
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                // Validate stream
                if (stream == null || stream.Length == 0)
                    return "error: No file data provided";

                // Ensure stream is seekable by copying to MemoryStream
                using (var memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;

                    // Basic file signature check
                    if (!IsValidExcelFile(memoryStream))
                        return "error: Invalid Excel file format";

                    memoryStream.Position = 0;
                    using (var reader = ExcelReaderFactory.CreateReader(memoryStream))
                    {
                        bool isHeaderSkipped = false;
                        List<LearningPlan> recordsToInsert = new();

                        if (!reader.Read())
                            return "error: Empty Excel file";

                        do
                        {
                            while (reader.Read())
                            {
                                if (!isHeaderSkipped)
                                {
                                    isHeaderSkipped = true;
                                    continue;
                                }

                                if (reader.FieldCount < 5)
                                    continue;

                                var name = reader.GetValue(2)?.ToString()?.Trim();
                                if (string.IsNullOrWhiteSpace(name))
                                    continue;

                                var dateString = reader.GetValue(0)?.ToString();
                                DateTime parsedDate = ParseThaiDate(dateString);
                                if (parsedDate == DateTime.MinValue)
                                    continue;

                                string book = GetBookNumber(name);

                                var newRecord = new LearningPlan
                                {
                                    Date = parsedDate,
                                    Time = reader.GetValue(1)?.ToString()?.Trim(),
                                    Name = name,
                                    Tape = reader.GetValue(3)?.ToString()?.Trim(),
                                    Lecturer = reader.GetValue(4)?.ToString()?.Trim(),
                                    ClassStudiedId = classStudiedId,
                                    Book = book,
                                    Score = 1,
                                    Status = true
                                };

                                recordsToInsert.Add(newRecord);
                            }
                        } while (reader.NextResult());

                        if (!recordsToInsert.Any())
                            return "error: No valid records found in Excel file";

                        _context.LearningPlans.AddRange(recordsToInsert);
                        await _context.SaveChangesAsync();

                        return "success";
                    }
                }
            }
            catch (Exception ex)
            {
                // Log for debugging
                Console.WriteLine($"UploadExcelAsync Error: {ex}");
                return $"error: {ex.Message}";
            }
        }

        private bool IsValidExcelFile(Stream stream)
        {
            try
            {
                byte[] buffer = new byte[4];
                stream.Read(buffer, 0, 4);
                stream.Position = 0;

                // Check for .xlsx (ZIP-based, starts with PKZIP signature)
                if (buffer[0] == 0x50 && buffer[1] == 0x4B && buffer[2] == 0x03 && buffer[3] == 0x04)
                    return true;

                // Check for .xls (older binary format, common signatures)
                if (buffer[0] == 0xD0 && buffer[1] == 0xCF && buffer[2] == 0x11 && buffer[3] == 0xE0)
                    return true;

                return false;
            }
            catch
            {
                return false;
            }
        }

        private DateTime ParseThaiDate(string dateString)
        {
            if (string.IsNullOrWhiteSpace(dateString))
                return default;

            var thCulture = new System.Globalization.CultureInfo("th-TH");

            if (DateTime.TryParse(dateString, thCulture, System.Globalization.DateTimeStyles.None, out DateTime thaiDate))
            {
                return thaiDate;
            }

            if (DateTime.TryParse(dateString, out DateTime standardDate))
            {
                return standardDate;
            }

            return default;
        }

        private string GetBookNumber(string topic)
        {
            var match = Regex.Match(topic, @"^(\d+)\.\d+\.\d+");
            if (match.Success)
            {
                return $"{match.Groups[1].Value}";
            }
            return null;
        }

        public async Task<(bool Success, string Message)> SaveLearningPlan(LearningPlan learningPlan, bool forceSave = false)
        {
            // ตรวจสอบว่าหัวข้อนี้มีอาจารย์สอนซ้ำหรือไม่
            bool instructorTopicExists = await _context.LearningPlans.AnyAsync(t =>
                t.Teacher_ID == learningPlan.Teacher_ID &&
                t.Name == learningPlan.Name &&
                t.Id != learningPlan.Id);

            if (instructorTopicExists && !forceSave)
            {
                return (false, "พบว่าอาจารย์ท่านนี้เคยสอนหัวข้อนี้แล้ว คุณต้องการบันทึกซ้ำหรือไม่?");
            }

            if (learningPlan.Id > 0)
            {
                var existing = await _context.LearningPlans.FindAsync(learningPlan.Id);
                if (existing != null)
                {
                    existing.Date = learningPlan.Date;
                    existing.Time = learningPlan.Time;
                    existing.Name = learningPlan.Name;
                    existing.Tape = learningPlan.Tape;
                    existing.Lecturer = learningPlan.Lecturer;
                    existing.Book = learningPlan.Book;
                    existing.Score = learningPlan.Score;
                    existing.Status = learningPlan.Status;
                    existing.Teacher_ID = learningPlan.Teacher_ID;
                    existing.ClassStudiedId = learningPlan.ClassStudiedId;
                }
            }
            else
            {
                _context.LearningPlans.Add(learningPlan);
            }

            await _context.SaveChangesAsync();
            return (true, learningPlan.Id > 0 ? "อัปเดตข้อมูลสำเร็จ" : "เพิ่มข้อมูลสำเร็จ");
        }

     
    }
}
