namespace ProjectBlazorManage1.Service.Interface
{
    public interface IInitialService
    {
        Task<List<Initial>> GetInitials();
        Task<Initial> GetInitialById(int id);
        Task AddInitial(Initial initial);
        Task UpdateInitial(Initial initial);
        Task Delete(int id);
    }
}
