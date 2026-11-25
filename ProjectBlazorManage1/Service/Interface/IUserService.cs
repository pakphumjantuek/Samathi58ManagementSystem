namespace ProjectBlazorManage1.Service.Interface
{
    public interface IUserService
    {
        Task<List<UserRoleViewModel>> GetAllUsersWithRolesAsync();
        Task<bool> ChangeUserRoleAsync(string userName, string newRole);
    }
}

