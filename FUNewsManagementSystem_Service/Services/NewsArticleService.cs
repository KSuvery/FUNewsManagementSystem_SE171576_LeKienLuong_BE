using FUNewsManagementSystem.Repo.DTO;
using FUNewsManagementSystem.Repo.Mapper;
using FUNewsManagementSystem.Repo.Repositories;

namespace FUNewsManagementSystem_Service.Services
{
    public interface INewsArticleService
    {
        Task<NewsArticleDTO> CreateNewsArticle(int userId, CreateNewsArticleRequest request);
        Task<bool> UpdateNewsArticle(int userId, int id, UpdateNewsArticleRequest request);
        Task<bool> DeleteNewsArticle(int id);
        Task<IEnumerable<NewsArticleDTO>> GetAllNewsArticle();
        Task<NewsArticleDTO?> GetNewsArticleById(int id);
    }
    public class NewsArticleService : INewsArticleService
    {
        private readonly IMapperlyMapper _mapper;
        private readonly INewsArticleRepository _repo;

        public NewsArticleService(IMapperlyMapper mapper, INewsArticleRepository repo)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<NewsArticleDTO> CreateNewsArticle(int userId, CreateNewsArticleRequest request)
        {
            try
            {
                var newsArticle = _mapper.MapCreateNewsArticleRequestToNewsArticle(request);
                newsArticle.CreatedById = userId;
                newsArticle.NewsStatus = true;

                await _repo.CreateNewsArticle(newsArticle);

                return _mapper.MapToNewsArticleDTO(newsArticle);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateNewsArticle(int userId, int id, UpdateNewsArticleRequest request)
        {
            try
            {
                var existingNews = await _repo.GetNewsArticleById(id);
                if (existingNews == null) return false;

                var newsDTO = _mapper.UpdateRequestMapToNewsArticleDTO(request);
                var newsArticle = _mapper.MapToNewsArticle(newsDTO);
                newsArticle.NewsArticleId = id;
                newsArticle.ModifiedDate = DateTime.Now;
                newsArticle.UpdatedById = userId;

                return await _repo.UpdateNewsArticle(newsArticle);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteNewsArticle(int id)
        {
            try
            {
                return await _repo.DeleteNewsArticle(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<NewsArticleDTO>> GetAllNewsArticle()
        {
            //try
            //{
            //    var news = await _repo.GetAllNewsArticle();
            //    return news.Select(n => _mapper.MapToNewsArticleDTO(n));
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
            var news = await _repo.GetAllNewsArticle();
            
            var newsDTOs = news.Select(n => new NewsArticleDTO
            {
                NewsArticleId = n.NewsArticleId,
                NewsTitle = n.NewsTitle,
                Headline = n.Headline,
                CreatedDate = n.CreatedDate,
                NewsContent = n.NewsContent,
                NewsSource = n.NewsSource,
                CreatedBy = n.CreatedBy?.AccountName,
                UpdatedBy = n.UpdatedBy?.AccountName,
                Category = n.Category?.CategoryName,
                NewsStatus = n.NewsStatus
            });

            return newsDTOs;
        }

        public async Task<NewsArticleDTO?> GetNewsArticleById(int id)
        {
            try
            {
                var news = await _repo.GetNewsArticleById(id);
                if (news == null) return null;

                return _mapper.MapToNewsArticleDTO(news);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
