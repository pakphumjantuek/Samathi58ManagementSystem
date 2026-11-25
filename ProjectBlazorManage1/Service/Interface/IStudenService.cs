namespace ProjectBlazorManage1.Service.Interface
{
    public interface IStudenService
    {
        Task<List<Students>> GetStudents();
        Task<Students> GetStudentId(int id);
        Task AddStudent (Students student);
        Task UpdateStudent (Students student);
        Task DeleteStudent (int id);
    }
}
