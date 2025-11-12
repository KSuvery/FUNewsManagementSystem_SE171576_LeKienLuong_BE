using FUNewsManagementSystem.Repo.Base;
using FUNewsManagementSystem.Repo.Models;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagementSystem.Repo.Repositories
{
    public interface INewsArticleRepository
    {
        Task<IEnumerable<NewsArticle>> GetAllNewsArticle();
        Task<NewsArticle> GetNewsArticleById(int id);
        Task<NewsArticle> CreateNewsArticle(NewsArticle newsArticle);
        Task<bool> UpdateNewsArticle(NewsArticle newsArticle);
        Task<bool> DeleteNewsArticle(int id);
    }

    public class NewsArticleRepository : GenericRepository<NewsArticle>, INewsArticleRepository
    {
        public NewsArticleRepository(NewsManagementDBContext context) : base(context)
        {
        }

        public async Task<NewsArticle> CreateNewsArticle(NewsArticle newsArticle)
        {
            newsArticle.CreatedDate = DateTime.Now;

            await CreateAsync(newsArticle);
            return newsArticle;
        }

        public async Task<bool> DeleteNewsArticle(int id)
        {
            var newsArticle = await GetByIdAsync(id);
            if (newsArticle == null) return false;

            newsArticle.NewsStatus = false;


            await UpdateAsync(newsArticle);
            return true;
        }

        public async Task<bool> UpdateNewsArticle(NewsArticle newsArticle)
        {
            newsArticle.ModifiedDate = DateTime.Now;

            await UpdateAsync(newsArticle);
            return true;
        }

        public async Task<IEnumerable<NewsArticle>> GetAllNewsArticle()
        {
            var newsArticles = await _context.NewsArticle
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.UpdatedBy)
                .Include(n => n.Tag)
                .Where(n => n.NewsStatus == true)
                .ToListAsync();
            return newsArticles;
        }

        public async Task<NewsArticle?> GetNewsArticleById(int id)
        {
            var newsArticle = await _context.NewsArticle
                .Where(n => n.NewsStatus == true)
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.UpdatedBy)
                .Include(n => n.Tag)
                .FirstOrDefaultAsync(n => n.NewsArticleId == id);
            return newsArticle;
        }
    }
}

