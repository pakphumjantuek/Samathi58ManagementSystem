namespace ProjectBlazorManage1.Model.Dtos
{
    public class RoleDto
    {
        [DisplayName("ชื่อ")]
        [Required(ErrorMessage = "กรุณากรอกชื่อบทบาท")]
        public string Name { get; set; }

    }
}
