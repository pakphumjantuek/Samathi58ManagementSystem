namespace ProjectBlazorManage1.Model
{
    public class Teacher
    {
        [Key]
        public int Teacher_Id { get; set; }
        [Required(ErrorMessage ="กรุณากรอกชื่อ")]
        [DisplayName("ชื่อ")]
        public string Teacher_Name { get; set; }
        [Required(ErrorMessage = "กรุณากรอกนามสกุล")]
        [DisplayName("นามสกุล")]
        public string Teacher_LastName { get; set; }
        [DisplayName("เบอร์โทรศัพท์")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "กรูณากรอกเบอร์โทร์ 10 หลัก")]
        [Required(ErrorMessage = "กรุณากรอกเบอร์โทรศัพท์")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "เบอร์โทรศัพท์ต้องขึ้นต้นด้วย 0")]
        public string Phone_Number { get; set; }
        [DisplayName("ที่อยู่")]
        [Required(ErrorMessage = "กรุณากรอกที่อยู่")]
        public string Address { get; set; }
        [DisplayName("รูปภาพ")]
        public string? Images { get; set; }
        [DisplayName("อาชัพปัจจุบัน")]
        [Required(ErrorMessage = "กรุณากรอกอาชัพปัจจุบัน")]
        public string Occupation { get; set; }
        [Required(ErrorMessage = "กรุณาเลือกคำนำหน้าชื่อ")]
        public int Initial_Id { get; set; }
        [ValidateNever]
        [ForeignKey("Initial_Id")]
        public Initial Initial { get; set; }
        //FK สาขา
        //[Required(ErrorMessage = "กรุณาเลือกสาขา")]
        public int? Branch_Id { get; set; }
        [ValidateNever]
        [ForeignKey("Branch_Id")]
        public Branch Branch { get; set; }
        [DisplayName("ตำเเหน่งงาน")]
        //[Required(ErrorMessage = "กรุณาเลือกตำเเหน่งงาน")]
        public int? JobPosition_Id { get; set; }
        [ValidateNever]
        [ForeignKey("JobPosition_Id")]
        public JobPosition jobPosition { get; set; }
        [DisplayName("ตำเเหน่งเพิ่มเติม")]
        [Required(ErrorMessage = "กรุณากรอกตำเเหน่งเพิ่มเติม")]
        public string? Position_detail { get; set; }
        [DisplayName("ความสามารถพิเศษ")]
        [Required(ErrorMessage = "กรุณากรอกความสามารถพิเศษ")]
        public string? Special_Skill { get; set; }

        public virtual ICollection<EducationHistory> EducationHistories { get; set; }
        public virtual ICollection<DhammaCourse> DhamaHistories { get; set; }

    }
}
