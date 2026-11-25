namespace ProjectBlazorManage1.Service.Interface
{
    public interface IPersonnelService
    {
        Task<List<Personnel>> GetAllPersonels();
        Task<Personnel> GetPersonnelById(int id);
        Task AddPersonnel(Personnel personnel);
        Task UpdatePersonnel(Personnel personnel);
        Task DeletePersonnel(int id);
    }
}
