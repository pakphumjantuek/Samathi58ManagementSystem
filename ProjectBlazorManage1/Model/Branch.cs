namespace ProjectBlazorManage1.Model
{
    public class Branch
    {
        [Key]
        [DisplayName("รหัสสาขา")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required(ErrorMessage = "กรุณากรอกรหัสสาขา")]
        public int Branch_Id { get; set; }
        [Required(ErrorMessage = "กรุณากรอกชื่อสาขา")]
        public string Branch_Name { get; set; }
        [DisplayName("เบอร์โทรศัพท์")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "กรูณากรอกเบอร์โทร์ 10 หลัก")]
        [Required(ErrorMessage = "กรุณากรอกเบอร์โทรศัพท์")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "เบอร์โทรศัพท์ต้องขึ้นต้นด้วย 0")]
        public string Branch_Phone { get; set; }
        [DisplayName("อีเมล")]
        [EmailAddress(ErrorMessage = "กรุณากรอกอีเมลให้ถูกต้อง")]
        [Required(ErrorMessage = "กรุณากรอกอีเมล")]
    
        public string Branch_Email { get; set; }
        [Required]
        [DisplayName("สถานะ")]
        public bool Status { get; set; } 

    }
}
