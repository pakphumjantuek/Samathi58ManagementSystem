namespace ProjectBlazorManage1.Model
{
    public class Initial
    {
        [Key]
        public int Initial_Id { get; set; }
        [DisplayName("คํานําหน้าชื่อภาษาไทย")]
        [Required(ErrorMessage = "กรุณากรอกชื่อคำนำหน้าชื่อ")]
        public string Initial_Name { get; set; }    
    }
}
