namespace ProjectBlazorManage1.Service.Interface
{
    public interface IDhammaCourseService
    {
        Task AddDhamma(DhammaCourse dhammaCourse);
        Task UpdateDhamma(DhammaCourse dhammaCourse);
        Task DeleteDhamma(int id);
    }
}
