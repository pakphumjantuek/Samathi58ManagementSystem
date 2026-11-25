
using Microsoft.AspNetCore.Components.Forms;

namespace ProjectBlazorManage1.Service
{
    public class NewsService : INewsService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BranchService> logger;
        private readonly string _imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/News");
        public NewsService(ApplicationDbContext Context, ILogger<BranchService> logger)
        {
            _context = Context;
            this.logger = logger;
        }
        public async Task<List<News>> GetAllNews()
        {
            return await _context.News.AsNoTracking().ToListAsync();
        }
        public async Task<News> FindIdNews(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ไอดีคำนำหน้าชื่อต้องเป็นค่าที่มากกว่า 0");
            }
            try
            {
                var FindNews = await _context.News.FindAsync(id);

                return FindNews ?? throw new KeyNotFoundException($"ไม่พบคำนำหน้าชื่อด้วยรหัส {id}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"เกิดข้อผิดพลาดขณะดึงข้อมูลคำนำหน้าชื่อ {id}");
                throw;
            }
        }
        public async Task AddNews(News news)
        {
            if (news == null)
            {
                throw new ArgumentNullException(nameof(news));
            }
            try
            {
                await _context.News.AddAsync(news);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                logger.LogError(ex, "เกิดข้อผิดพลาดขณะเพิ่มใหม่");
                throw new InvalidOperationException("ไม่สามารถเพิ่มได้ อาจมีข้อมูลซ้ำ", ex);
            }
        }
        public async Task UpdateNews(News news)
        {
            var existingNews = await FindIdNews(news.News_Id);
            if (existingNews != null)
            {
                existingNews.Title = news.Title;
                existingNews.News_Content = news.News_Content;
                existingNews.DateStart = DateTime.Now;
                existingNews.Images = news.Images;
                existingNews.YouTubeUrl = news.YouTubeUrl;
                existingNews.Status = news.Status;

                await _context.SaveChangesAsync();
            }
        }
        public async Task DeleteNews(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("รหัสข้อมูลต้องเป็นค่าที่มากกว่า 0");
            }

            try
            {
                var NewsId = await _context.News.FindAsync(id);
                if (NewsId == null)
                {
                    throw new KeyNotFoundException($"ไม่พบด้วยรหัส {id}");
                }

                _context.News.Remove(NewsId);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                logger.LogError(ex, $"เกิดข้อผิดพลาดขณะลบข้อมูล {id}");
                throw new InvalidOperationException("ไม่สามารถลบข้อมูลได้ ข้อมูลอาจถูกใช้งานอยู่", ex);
            }
        }

        public async Task<string> UploadImage(IBrowserFile file)
        {
            try
            {
                if (!Directory.Exists(_imagePath))
                {
                    Directory.CreateDirectory(_imagePath);
                }

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.Name)}";
                var filePath = Path.Combine(_imagePath, fileName);

                using var fileStream = new FileStream(filePath, FileMode.Create);
                await file.OpenReadStream(10 * 1024 * 1024).CopyToAsync(fileStream);

                return $"/images/News/{fileName}";
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to upload image: {ex.Message}");
            }
        }

        public async Task DeleteImage(string imageUrl)
        {
            try
            {
                var fileName = Path.GetFileName(imageUrl);
                var filePath = Path.Combine(_imagePath, fileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete image: {ex.Message}");
            }
        }

    }

}

