namespace ProjectBlazorManage1.Service.Interface
{
    public interface ILearningPlanService
    {
        Task<List<LearningPlan>> GetAllAsync();
        Task<LearningPlan> GetByIdAsync(int id);
        Task AddLearningPlan(LearningPlan learningPlan);
        Task UpdateLearningPlan(LearningPlan learningPlan);
        Task DeleteAsync(int id);
    }
}
