using ProjectBlazorManage1.Model;

namespace ProjectBlazorManage1.Service.Interface
{
    public interface IBranchService
    {
        Task<List<Branch>> GetBranches();
        Task<Branch> GetBranchById(int id);
        Task AddBranch(Branch branch);
        Task UpdateBranch(Branch branch);
        Task DeleteBranch(int id);

    }
}
