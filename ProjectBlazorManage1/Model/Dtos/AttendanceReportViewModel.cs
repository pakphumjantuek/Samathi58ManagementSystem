namespace ProjectBlazorManage1.Model.Dtos
{
    public class AttendanceReportViewModel
    {
        public int ClassId { get; set; }
        public int ClassNumber { get; set; }

        public List<Students> Students { get; set; } = new();
        public List<LearningPlan> LearningPlans { get; set; } = new();
        public List<Attendance> AttendanceRecords { get; set; } = new();

        // TotalScore ของแต่ละนักเรียน: Key = Student.Id, Value = คะแนนรวม
        public Dictionary<int, double> StudentScores { get; set; } = new();

        public (string Type, string Remark) GetAttendanceStatus(int studentId, int planId)
        {
            var record = AttendanceRecords.FirstOrDefault(a => a.StudentId == studentId && a.LearningPlanId == planId);
            string type = record?.StudiedType switch
            {
                StudiedType.มาเรียน => "present",
                StudiedType.เรียนเสริม => "extra",
                StudiedType.เรียนต่างสาขา => "branch",
                _ => "absent"
            };
            return (type, record?.Remarks ?? "");
        }
    }
}
