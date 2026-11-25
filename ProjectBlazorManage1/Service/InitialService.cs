
namespace ProjectBlazorManage1.Service
{
    public class InitialService : IInitialService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BranchService> _logger;

        public InitialService(ApplicationDbContext context, ILogger<BranchService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<List<Initial>> GetInitials()
        {
            try
            {
                return await _context.Initials.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "เกิดข้อผิดพลาดขณะดึงข้อมูลคำนำหน้าชื่อทั้งหมด");
                throw;
            }
        }
        public async Task<Initial> GetInitialById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ไอดีคำนำหน้าชื่อต้องเป็นค่าที่มากกว่า 0");
            }
            try
            {
                var branch = await _context.Initials
                    .AsNoTracking()
                    .FirstOrDefaultAsync(b => b.Initial_Id == id);

                return branch ?? throw new KeyNotFoundException($"ไม่พบคำนำหน้าชื่อด้วยรหัส {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"เกิดข้อผิดพลาดขณะดึงข้อมูลคำนำหน้าชื่อ {id}");
                throw;
            }
        }
        public async Task AddInitial(Initial initial)
        {
            if (initial == null)
            {
                throw new ArgumentNullException(nameof(initial));
            }
            try
            {
                ValidateInitial(initial);
                await _context.Initials.AddAsync(initial);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "เกิดข้อผิดพลาดขณะเพิ่มสาขาใหม่");
                throw new InvalidOperationException("ไม่สามารถเพิ่มคำนำหน้าชื่อได้ อาจมีข้อมูลซ้ำ", ex);
            }
        }
        public async Task UpdateInitial(Initial initial)
        {
            var existingInitial = _context.Initials.FirstOrDefault(b => b.Initial_Id == initial.Initial_Id);
            if (existingInitial != null)
            {
                existingInitial.Initial_Name = initial.Initial_Name;

                await _context.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("รหัสสาขาต้องเป็นค่าที่มากกว่า 0");
            }

            try
            {
                var initial = await _context.Initials.FindAsync(id);
                if (initial == null)
                {
                    throw new KeyNotFoundException($"ไม่พบคำนำหน้าด้วยรหัส {id}");
                }

                _context.Initials.Remove(initial);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"เกิดข้อผิดพลาดขณะลบคำนำหน้าชื่อ {id}");
                throw new InvalidOperationException("ไม่สามารถลบคำนำหน้าชื่อได้ ข้อมูลอาจถูกใช้งานอยู่", ex);
            }
        }

        private void ValidateInitial(Initial initial)
        {
            if (string.IsNullOrWhiteSpace(initial.Initial_Name))
            {
                throw new ArgumentException("ชื่อคำนำหน้าไม่สามารถเป็นค่าว่าง");
            }
        }

    }
}
