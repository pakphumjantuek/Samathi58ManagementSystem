namespace ProjectBlazorManage1.Service.Interface
{
    public interface iJobPositionService
    {
        Task<List<JobPosition>> GetJobPS();
        Task<JobPosition> GetJobID(int id);
        Task AddJobPs(JobPosition jobPosition);
        Task UpdateJobPs(JobPosition jobPosition);
        Task DeleteJobPs(int id);
    }
}
