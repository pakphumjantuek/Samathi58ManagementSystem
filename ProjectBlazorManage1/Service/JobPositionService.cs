namespace ProjectBlazorManage1.Service
{
    public class JobPositionService : iJobPositionService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BranchService> _logger;
        public JobPositionService(ApplicationDbContext context, ILogger<BranchService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<List<JobPosition>> GetJobPS()
        {
          return await _context.JobPositions.AsNoTracking().ToListAsync();
        }
        public async Task<JobPosition> GetJobID(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentException("ไอดีต้องเป็นค่าที่มากกว่า 0");
            }

            try
            {
                var JobPS = await _context.JobPositions
                    .AsNoTracking()
                    .FirstOrDefaultAsync(b => b.Id == id);

                return JobPS ?? throw new KeyNotFoundException($"ไม่พบตำเเหน่งงานด้วยรหัส {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"เกิดข้อผิดพลาดขณะดึงข้อมูลตำเเหน่งงานรหัส {id}");
                throw;
            }
        }
        public async Task AddJobPs(JobPosition jobPosition)
        {
            if (jobPosition == null)
            {
                throw new ArgumentNullException(nameof(jobPosition));
            }
            try
            {
                await _context.JobPositions.AddAsync(jobPosition);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "เกิดข้อผิดพลาดขณะเพิ่มตำเเหน่งงานใหม่");
                throw new InvalidOperationException("ไม่สามารถตำเเหน่งงานได้ อาจมีข้อมูลซ้ำ", ex);
            }
        }
        public async Task UpdateJobPs(JobPosition jobPosition)
        {
            var existingJobPs = _context.JobPositions.FirstOrDefault(b => b.Id == jobPosition.Id);
            if (existingJobPs != null)
            {
                existingJobPs.Position_Name = jobPosition.Position_Name;
                existingJobPs.Position_Level = jobPosition.Position_Level;
                existingJobPs.Status = jobPosition.Status;
                await _context.SaveChangesAsync();
            }
        }
        public async Task DeleteJobPs(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("รหัสสาขาต้องเป็นค่าที่มากกว่า 0");
            }
            try
            {
                var JobPS = await _context.JobPositions.FindAsync(id);
                if (JobPS == null)
                {
                    throw new KeyNotFoundException($"ไม่พบตำเเหน่งงานด้วยรหัส {id}");
                }
                _context.JobPositions.Remove(JobPS);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"เกิดข้อผิดพลาดขณะลบตำเเหน่งงานรหัส {id}");
                throw new InvalidOperationException("ไม่สามารถตำเเหน่งงานได้ ข้อมูลอาจถูกใช้งานอยู่", ex);
            }
        }
      
    }
}
