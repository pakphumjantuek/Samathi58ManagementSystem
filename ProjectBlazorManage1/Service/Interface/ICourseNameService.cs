namespace ProjectBlazorManage1.Service.Interface
{
    public interface ICourseNameService
    {
        Task<List<CourseName>> GetAllCourseNames();
        Task<CourseName> GetCourseNameById(int id);
        Task AddCourseNames(CourseName courseName);
        Task UpdateCourseName(CourseName courseName);
        Task DeleteCourseName(int id);

    }
}
