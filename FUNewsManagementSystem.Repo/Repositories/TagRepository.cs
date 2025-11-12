using FUNewsManagementSystem.Repo.Base;
using FUNewsManagementSystem.Repo.Models;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagementSystem.Repo.Repositories
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllTags();
        Task<Tag> GetTagById(int id);
        Task<Tag> CreateTag(Tag tag);
        Task<bool> UpdateTag(Tag tag);
        Task<bool> DeleteTag(int id);
    }

    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        public TagRepository(NewsManagementDBContext context) : base(context)
        {
        }

        public async Task<Tag> CreateTag(Tag tag)
        {
            try
            {
                await CreateAsync(tag);
                return tag;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the tag.", ex);
            }
        }

        public async Task<bool> DeleteTag(int id)
        {
            try
            {
                var tag = await GetByIdAsync(id);
                if (tag == null)
                {
                    return false;
                }

                await RemoveAsync(tag);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the tag.", ex);
            }
        }

        public async Task<IEnumerable<Tag>> GetAllTags()
        {
            try
            {
                var tags = await _context.Tag.Include(t => t.NewsArticle).ToListAsync();
                return tags;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving tags.", ex);
            }
        }

        public async Task<Tag?> GetTagById(int id)
        {
            try
            {
                var tag = await _context.Tag.Include(t => t.NewsArticle).Where(t => t.TagId == id).FirstOrDefaultAsync();
                return tag;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving tags.", ex);
            }
        }

        public async Task<bool> UpdateTag(Tag tag)
        {
            try
            {
                await UpdateAsync(tag);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving tags.", ex);
            }
        }
    }
}
