namespace ProjectBlazorManage1.Model.Dtos
{
    public class AttendanceViewModel
    {
        public int ClassId { get; set; }
        public int LearningPlanId { get; set; }
        public string LearningPlanName { get; set; }
        public int ClassNumber { get; set; }
        public DateTime Date { get; set; }

        public List<Students> Students { get; set; } = new List<Students>();

        // ใช้สำหรับดึงข้อมูลการเช็คชื่อที่มีอยู่แล้ว (กรณี Edit)
        public Dictionary<int, Attendance> ExistingAttendance { get; set; } = new();

        // รายการเช็คชื่อที่จะบันทึก
        public List<Attendance> AttendanceRecords { get; set; } = new();
    }
}
