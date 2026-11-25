namespace ProjectBlazorManage1.Model
{
    public class ClassStudied
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DisplayName("รุ่นที่เรียน")]
        [Range(1, int.MaxValue, ErrorMessage = "รหัสรุ่นต้องมากกว่า 0")]
        public int Id { get; set; }
        [DisplayName("วันที่เริ่มเรียน")]
        [Required(ErrorMessage = "กรุณาเลือกวันที่เริ่มเรียน")]
        public DateTime? StartDate { get; set; } = DateTime.Today;
        [DisplayName("วันที่สิ้นสุดการเรียน")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "กรุณาเลือกวันที่สิ้นสุดการเรียน")]
        public DateTime? EndDate { get; set; } = DateTime.Today;
        [DisplayName("สถานะ")]
        public bool Status { get; set; }
        [DisplayName("ชื่อรุ่น")]
        [Required(ErrorMessage = "กรุณาใส่ชื่อรุ่น")]
        public string ClassName { get; set; } = string.Empty; // ชื่อรุ่น
        [DisplayName("ภาคเรียน")]
        [Required(ErrorMessage = "กรุณาเลือกภาคเรียน")]
        public string termClass { get; set; } = string.Empty; // ภาคเรียน
        [Required(ErrorMessage = "กรุณาเลือกสาขา")]
        public int? Branch_Id { get; set; }
        [ValidateNever]
        [ForeignKey("Branch_Id")]
        public Branch Branch { get; set; }

        [NotMapped]
        public LearningPlan? learningPlan { get; set; }
        public List<LearningPlan> learningPlans { get; set; } 

        public List<string> GetStudyMonths(DateTime? StartDate, DateTime? EndDate)
        {
            List<string> months = new List<string>();

            if (StartDate == null || EndDate == null)
                return months; // หรือ throw exception หรือให้ค่า default ตามต้องการ

            DateTime current = new DateTime(StartDate.Value.Year, StartDate.Value.Month, 1);
            DateTime end = new DateTime(EndDate.Value.Year, EndDate.Value.Month, 1);

            while (current <= end && months.Count < 6)
            {
                months.Add(current.ToString("MMMM yyyy", new System.Globalization.CultureInfo("th-TH")));
                current = current.AddMonths(1);
            }

            return months;
        }

        public bool IsStudyPeriodValid()
        {
            if (StartDate == null || EndDate == null)
                return false;

            if (EndDate < StartDate)
                return false; // ห้ามย้อนเวลา

            // นับจำนวนเดือนห่างกันแบบรวมเดือนเริ่มต้นด้วย
            int totalMonths = ((EndDate.Value.Year - StartDate.Value.Year) * 12)
                            + (EndDate.Value.Month - StartDate.Value.Month) + 1;

            return totalMonths == 6;
        }


    }
}
