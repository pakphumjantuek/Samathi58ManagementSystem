namespace ProjectBlazorManage1.Model.Dtos
{
    public class UserRoleViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string CurrentRoles { get; set; }
        public string SelectedRole { get; set; }
        public List<string> Roles { get; set; } = new();
        public string ProfileImageUrl { get; set; }


    }

}
