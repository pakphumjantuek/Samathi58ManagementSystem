namespace ProjectBlazorManage1.Model.Dtos
{
    public class RoleUpdateDto : RoleDto
    {

        [DisplayName("ชื่อใหม่")]
        public string UpdateName { get; set; }
    }
}
