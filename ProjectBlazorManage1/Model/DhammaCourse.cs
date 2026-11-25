namespace ProjectBlazorManage1.Model
{
    public class DhammaCourse
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("รุ่นที่เรียน")]
        public int generation { get; set; } //รุ่นที่
        [DisplayName("หลักสูตร")]
        [Required(ErrorMessage = "กรุณาเลือกหลักสูตร")]
        public int CourseName_Id { get; set; } //หัวข้อ
        [ValidateNever]
        [ForeignKey("CourseName_Id")]
        public CourseName CourseName { get; set; }
        public int? TeacherId { get; set; }
        [ValidateNever]
        [ForeignKey("TeacherId")]
        public virtual Teacher Teacher { get; set; }
        public int? Personnel_Id { get; set; }
        [ValidateNever]
        [ForeignKey("Personnel_Id")]
        public Personnel Personnel { get; set; }
    }
}
