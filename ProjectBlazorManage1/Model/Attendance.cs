namespace ProjectBlazorManage1.Model
{
    public class Attendance
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int LearningPlanId { get; set; }
        public DateTime CheckInTime { get; set; }
        public StudiedType StudiedType { get; set; } 
        public bool Extraclass { get; set; } // เรียนเสริมพิเศษ
        public string? Remarks { get; set; }

        [ForeignKey("StudentId")]
        [ValidateNever]
        public Students Students { get; set; }
        [ForeignKey("LearningPlanId")]
        [ValidateNever]
        public LearningPlan LearningPlan { get; set; }
    }
}
