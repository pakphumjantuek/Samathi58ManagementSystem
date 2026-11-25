namespace ProjectBlazorManage1.Service.Interface
{
    public interface INewsService
    {
        Task<List<News>> GetAllNews();
        Task<News> FindIdNews(int id);
        Task AddNews(News news);
        Task UpdateNews(News news);
        Task DeleteNews(int id);

    }
}
