
using ProjectBlazorManage1.Model;

namespace ProjectBlazorManage1.Service
{
    public class EducationHistoryService : IEducationHistorySerice
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BranchService> _logger;

        public EducationHistoryService(ApplicationDbContext context, ILogger<BranchService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }      
        public async Task CreateHistoryAsync(EducationHistory educationHistory)
        {
            if (educationHistory == null)
            {
                throw new ArgumentNullException(nameof(educationHistory));
            }
            try
            {
                await _context.EducationHistories.AddAsync(educationHistory);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "เกิดข้อผิดพลาดขณะเพิ่มข้อมูลใหม่");
                throw new InvalidOperationException("ไม่สามารถเพิ่มข้อมูลได้ อาจมีข้อมูลซ้ำ", ex);
            }
        }
        public async Task UpdateHistoryAsync(EducationHistory educationHistory)
        {
            var existingeducationHistory = _context.EducationHistories.FirstOrDefault(b => b.EducationHistoryId == educationHistory.EducationHistoryId);
            if (existingeducationHistory != null)
            {
                existingeducationHistory.Degree = educationHistory.Degree;
                existingeducationHistory.Institution = educationHistory.Institution;
                existingeducationHistory.MajorName = educationHistory.MajorName;
                existingeducationHistory.YearGraduated = educationHistory.YearGraduated;
                existingeducationHistory.degreeName = educationHistory.degreeName;
                existingeducationHistory.TeacherId = educationHistory.TeacherId;
               
                await _context.SaveChangesAsync();
            }
        }
        public async Task DeleteHistoryAsync(int id)
        {
            var findId = _context.EducationHistories.Find(id);
            if (findId != null)
            {
                _context.EducationHistories.Remove(findId);
                 await _context.SaveChangesAsync();
            }     
        }
     
    }
}
