namespace ProjectBlazorManage1.Model
{
    public class Personnel
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("คำนำหน้า")]
        public int Initial_Id { get; set; }
        [ForeignKey("Initial_Id")]
        [ValidateNever]
        public Initial initial { get; set; }
        [DisplayName("ชื่อ")]
        [Required(ErrorMessage = "โปรดกรอกชื่อ")]
        public string Personnel_Name { get; set; }
        [DisplayName("นามสกุล")]
        [Required(ErrorMessage = "โปรดกรอกนามสกุล")]
        public string Personnel_LastName { get; set; }
        [DisplayName("ที่อยู่")]
        public string? Personnel_Address { get; set; }
        [DisplayName("อีเมล")]
        [EmailAddress(ErrorMessage = "รูปแบบอีเมลไม่ถูกต้อง")]
        [DataType(DataType.EmailAddress)]
        public string? Personnel_Email { get; set; }
        [Required(ErrorMessage = "โปรดกรอกเบอร์โทร")]
        [DisplayName("เบอร์โทร")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "เบอร์โทรต้องประกอบด้วยตัวเลข 10 หลัก")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "เบอร์โทรต้องมีความยาว 10 หลัก")]
        public string Phone_Number { get; set; }
        [DisplayName("วันที่เริ่มงาน")]
        public DateTime? Date { get; set; } = DateTime.Today;
        [DisplayName("รูปภาพ")]
        public string? images { get; set; }
        [DisplayName("สถานะ")]
        public bool Status { get; set; } = false;

        [DisplayName("ตำเเหน่งงาน")]
        public int jobPosition_ID { get; set; }
        [ForeignKey("jobPosition_ID")]
        [ValidateNever]
        public JobPosition jobPosition { get; set; }

        public virtual ICollection<DhammaCourse> DhamaHistories { get; set; }

    }
}
