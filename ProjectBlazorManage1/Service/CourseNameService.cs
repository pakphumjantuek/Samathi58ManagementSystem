using ProjectBlazorManage1.Model;

namespace ProjectBlazorManage1.Service
{
    public class CourseNameService : ICourseNameService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BranchService> _logger;
        public List<CourseName> CourseNameList { get; set; }
        public CourseNameService(ApplicationDbContext context, ILogger<BranchService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            CourseNameList = new List<CourseName>();
        }

        public async Task<List<CourseName>> GetAllCourseNames()
        {
            try
            {
                return await _context.CourseNames.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "เกิดข้อผิดพลาดขณะดึงข้อมูลทั้งหมด");
                throw;
            }
        }
        public async Task<CourseName> GetCourseNameById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ไอดีคำนำหน้าชื่อต้องเป็นค่าที่มากกว่า 0");
            }
            try
            {
                var courseName = await _context.CourseNames
                    .AsNoTracking()
                    .FirstOrDefaultAsync(b => b.Id == id);

                return courseName ?? throw new KeyNotFoundException($"ไม่พบด้วยรหัส {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"เกิดข้อผิดพลาดขณะดึงข้อมูล {id}");
                throw;
            }
        }
        public async Task AddCourseNames(CourseName courseName)
        {
            if (courseName == null)
            {
                throw new ArgumentNullException(nameof(courseName));
            }
            try
            {
                await _context.CourseNames.AddAsync(courseName);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "เกิดข้อผิดพลาดขณะเพิ่มข้อมูลใหม่");
                throw new InvalidOperationException("ไม่สามารถเพิ่มข้อมูลได้ อาจมีข้อมูลซ้ำ", ex);
            };
        }
        public async Task UpdateCourseName(CourseName courseName)
        {
            var existCourseName = _context.CourseNames.FirstOrDefault(b => b.Id == courseName.Id);
            if (existCourseName != null)
            {
                existCourseName.Name = courseName.Name;
                existCourseName.Level = courseName.Level;
                existCourseName.Status = courseName.Status;


                await _context.SaveChangesAsync();
            }
        }
        public async Task DeleteCourseName(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("รหัสต้องเป็นค่าที่มากกว่า 0");
            }

            try
            {
                var courseName = await _context.CourseNames.FindAsync(id);
                if (courseName == null)
                {
                    throw new KeyNotFoundException($"ไม่พบรหัส {id}");
                }

                _context.CourseNames.Remove(courseName);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"เกิดข้อผิดพลาดขณะลบข้อมูล {id}");
                throw new InvalidOperationException("ไม่สามารถลบข้อได้ ข้อมูลอาจถูกใช้งานอยู่", ex);
            }
        }

      
    }
}
