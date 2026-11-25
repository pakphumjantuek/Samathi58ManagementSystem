
namespace ProjectBlazorManage1.Service
{
    public class BranchService : IBranchService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BranchService> _logger;

        public BranchService(ApplicationDbContext context, ILogger<BranchService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<Branch>> GetBranches()
        {
            try
            {
                return await _context.Branches
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "เกิดข้อผิดพลาดขณะดึงข้อมูลสาขาทั้งหมด");
                throw; 
            }
        }

        public async Task<Branch> GetBranchById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("รหัสสาขาต้องเป็นค่าที่มากกว่า 0");
            }

            try
            {
                var branch = await _context.Branches
                    .AsNoTracking()
                    .FirstOrDefaultAsync(b => b.Branch_Id == id);

                return branch ?? throw new KeyNotFoundException($"ไม่พบสาขาด้วยรหัส {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"เกิดข้อผิดพลาดขณะดึงข้อมูลสาขารหัส {id}");
                throw;
            }
        }

        public async Task AddBranch(Branch branch)
        {
            if (branch == null)
            {
                throw new ArgumentNullException(nameof(branch));
            }

            try
            {
                await _context.Branches.AddAsync(branch);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "เกิดข้อผิดพลาดขณะเพิ่มสาขาใหม่");
                throw new InvalidOperationException("ไม่สามารถเพิ่มสาขาได้ อาจมีข้อมูลซ้ำ", ex);
            }
        }
        public async Task UpdateBranch(Branch branch)
        {
            var existingBranch = _context.Branches.FirstOrDefault(b => b.Branch_Id == branch.Branch_Id );
            if (existingBranch != null)
            {
                existingBranch.Branch_Name = branch.Branch_Name;
                existingBranch.Branch_Phone = branch.Branch_Phone;
                existingBranch.Branch_Email = branch.Branch_Email;
                existingBranch.Status = branch.Status;

                // บันทึกการเปลี่ยนแปลงลงฐานข้อมูล
                await _context.SaveChangesAsync();
            }
        }
        public async Task DeleteBranch(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("รหัสสาขาต้องเป็นค่าที่มากกว่า 0");
            }

            try
            {
                var branch = await _context.Branches.FindAsync(id);
                if (branch == null)
                {
                    throw new KeyNotFoundException($"ไม่พบสาขาด้วยรหัส {id}");
                }

                _context.Branches.Remove(branch);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"เกิดข้อผิดพลาดขณะลบสาขารหัส {id}");
                throw new InvalidOperationException("ไม่สามารถลบสาขาได้ ข้อมูลอาจถูกใช้งานอยู่", ex);
            }
        }

    }
}