namespace ProjectBlazorManage1.Service.Interface
{
    public interface IClassStudieService
    {
        Task<List<ClassStudied>> GetClassStudies();
        Task<ClassStudied> GetClassStudyById(int id);
        Task AddClassStudy(ClassStudied classStudie);
        Task UpdateClassStudy(ClassStudied classStudie);
        Task DeleteClassStudy(int id);
    }
}
