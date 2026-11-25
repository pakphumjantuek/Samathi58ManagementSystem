using ProjectBlazorManage1.Model.Dtos;

namespace ProjectBlazorManage1.Service.Interface
{
    public interface IRoleservice
    {
        Task<List<IdentityRole>> GetAll();
        Task<bool> Add(RoleDto roleDto);
        Task<bool> Update(RoleUpdateDto roleUpdateDto);
        Task<bool> Delete(string name);
        Task<IdentityRole> Find(string name);
    }
}
