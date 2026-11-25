namespace ProjectBlazorManage1.Model
{
    public class LearningPlan
    {
        public int Id { get; set; }
        [DisplayName("วันที่เรียน")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "กรุณาเลือกวันที่เรียน")]
        public DateTime? Date { get; set; } = DateTime.Today; // วันที่เรียน
        [DisplayName("เวลาเรียน")]
        [Required(ErrorMessage = "กรุณาเลือกเวลาเรียน")]
        public string? Time { get; set; } = string.Empty; // เวลา
        [DisplayName("หัวข้อการเรียน")]
        [Required(ErrorMessage = "กรุณาเลือกหัวข้อการเรียน")]
        public string Name { get; set; } = string.Empty; // หัวข้อบรรยาย
        [DisplayName("เทป")]
        //[Required(ErrorMessage = "กรุณาเลือกเทป")]
        public string? Tape { get; set; } = string.Empty; // เทป
        [DisplayName("")]
        public string? Lecturer { get; set; } = string.Empty; // อาจารย์ผู้บรรยาย
        [DisplayName("เล่มที่")]
        //[Required(ErrorMessage = "กรุณาเลือกเล่มที่")]
        public string? Book { get; set; } = string.Empty; // หนังสือ
        [DisplayName("คะเเนน")]
        public double? Score { get; set; } = 1; // คะเเนน
        public bool? Status { get; set; } // สถานะ
        [DisplayName("อาจาร์ผู้สอน")]
        public int? Teacher_ID { get; set; }
        [ForeignKey("Teacher_ID")]
        [ValidateNever]
        public Teacher teacher { get; set; }
        [Required(ErrorMessage = "กรุณาเลือกรุ่น")]
        public int? ClassStudiedId { get; set; }
        [ForeignKey("ClassStudiedId")]
        [ValidateNever]
        public ClassStudied ClassStudied { get; set; }
    }
}
