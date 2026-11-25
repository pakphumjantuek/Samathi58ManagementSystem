
namespace ProjectBlazorManage1.Model.Dtos
{
    public class ClassReportResult
    {
        public ClassStudied ClassInfo { get; set; }
        public List<LearningPlan> LearningPlans { get; set; } = new();
        public List<Students> Students { get; set; } = new();
        public List<Attendance> AttendanceRecords { get; set; } = new();
        public Dictionary<int, StudentMetrics> StudentMetricsDict { get; set; } = new();
    }
}
