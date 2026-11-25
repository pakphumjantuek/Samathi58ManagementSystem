

using Microsoft.EntityFrameworkCore;

namespace ProjectBlazorManage1.Service
{
    public class DhammaCourseService : IDhammaCourseService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BranchService> _logger;
        public DhammaCourseService(ApplicationDbContext context, ILogger<BranchService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public async Task<List<DhammaCourse>> GetallDhama()
        {
           return  await _context.DhammaCourses
                .Include(p=> p.Teacher)
                .Include(p=> p.CourseName)
                .Include(p => p.Personnel)
                .ToListAsync();
        }
        public async Task<List<DhammaCourse>> GetallCourse()
        {
            return await _context.DhammaCourses
                 .Include(p => p.Teacher)
                 .Include(p => p.CourseName)
                 .Include(p => p.Personnel)
                 .ToListAsync();
        }

        public async Task AddDhamma(DhammaCourse dhammaCourse)
        {
            if (dhammaCourse == null)
            {
                throw new ArgumentNullException(nameof(dhammaCourse));
            }
            try
            {
                await _context.DhammaCourses.AddAsync(dhammaCourse);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "เกิดข้อผิดพลาดขณะเพิ่มข้อมูลใหม่");
                throw new InvalidOperationException("ไม่สามารถเพิ่มข้อมูลได้ อาจมีข้อมูลซ้ำ", ex);
            }
        }
        public async Task UpdateDhamma(DhammaCourse dhammaCourse)
        {
            var existingeDhamma = _context.DhammaCourses.FirstOrDefault(b => b.Id == dhammaCourse.Id);
            if (existingeDhamma != null)
            {
                existingeDhamma.generation = dhammaCourse.generation;
                existingeDhamma.CourseName_Id = dhammaCourse.CourseName_Id;
                existingeDhamma.TeacherId = dhammaCourse.TeacherId;
                existingeDhamma.Personnel_Id = dhammaCourse.Personnel_Id;

                await _context.SaveChangesAsync();
            }
        }
        public async Task DeleteDhamma(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("รหัสสาขาต้องเป็นค่าที่มากกว่า 0");
            }

            try
            {
                var dhamma = await _context.DhammaCourses.FindAsync(id);
                if (dhamma == null)
                {
                    throw new KeyNotFoundException($"ไม่พบข้อมูลด้วยรหัส {id}");
                }

                _context.DhammaCourses.Remove(dhamma);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"เกิดข้อผิดพลาดขณะลบข้อมูล {id}");
                throw new InvalidOperationException("ไม่สามารถลบข้อมูลได้ ข้อมูลอาจถูกใช้งานอยู่", ex);
            }
        }
  
      
    }
}
