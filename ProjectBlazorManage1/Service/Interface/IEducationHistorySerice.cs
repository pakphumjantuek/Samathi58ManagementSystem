namespace ProjectBlazorManage1.Service.Interface
{
    public interface IEducationHistorySerice
    {     
        Task CreateHistoryAsync(EducationHistory educationHistory);
        Task UpdateHistoryAsync(EducationHistory educationHistory);
        Task DeleteHistoryAsync(int id);

    }
}
