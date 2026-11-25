
namespace ProjectBlazorManage1.Service
{
    public class PersonnelService : IPersonnelService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PersonnelService> _logger;
        private readonly string _imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Personnel");

        public PersonnelService(ApplicationDbContext context, ILogger<PersonnelService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<List<Personnel>> GetAllPersonels()
        {
           return  await _context.Personnel.AsNoTracking()
                .Include(p => p.jobPosition)
                .Include(p => p.initial)
                .Include(p => p.DhamaHistories)
                 .ThenInclude(d => d.CourseName)
                .ToListAsync();
        }
        public async Task<Personnel> GetPersonnelById(int id)
        {
        return  await _context.Personnel
             .Include(p => p.DhamaHistories)
             .ThenInclude(h => h.CourseName)
             .Include(p => p.jobPosition)
             .Include(p => p.initial)
             .FirstOrDefaultAsync(p => p.Id == id);
        }


        public async Task AddPersonnel(Personnel personnel)
        {
            await _context.Personnel.AddAsync(personnel);
            await _context.SaveChangesAsync();
        }
        public async Task UpdatePersonnel(Personnel personnel)
        {
            var existingBranch = _context.Personnel.FirstOrDefault(b => b.Id == personnel.Id);
            if (existingBranch != null)
            {
                existingBranch.Personnel_Name = personnel.Personnel_Name;
                existingBranch.Personnel_LastName = personnel.Personnel_LastName;
                existingBranch.Personnel_Email = personnel.Personnel_Email;
                existingBranch.Personnel_Address = personnel.Personnel_Address;
                existingBranch.Phone_Number = personnel.Phone_Number;
                existingBranch.Date = personnel.Date;
                existingBranch.images = personnel.images;
                existingBranch.jobPosition_ID = personnel.jobPosition_ID;
                existingBranch.Initial_Id = personnel.Initial_Id;
                existingBranch.Status = personnel.Status;
              
                // บันทึกการเปลี่ยนแปลงลงฐานข้อมูล
                await _context.SaveChangesAsync();
            }
        }
        public async Task DeletePersonnel(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("รหัสต้องเป็นค่าที่มากกว่า 0");
            }

            try
            {
                var personnel = await _context.Personnel.FindAsync(id);
                if (personnel == null)
                {
                    throw new KeyNotFoundException($"ไม่พบด้วยรหัส {id}");
                }

                _context.Personnel.Remove(personnel);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"เกิดข้อผิดพลาดขณะลบรหัส {id}");
                throw new InvalidOperationException("ไม่สามารถลบได้ ข้อมูลอาจถูกใช้งานอยู่", ex);
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
