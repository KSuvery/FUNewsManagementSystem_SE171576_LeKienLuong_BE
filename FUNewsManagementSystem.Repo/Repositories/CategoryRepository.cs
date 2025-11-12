using FUNewsManagementSystem.Repo.Base;
using FUNewsManagementSystem.Repo.Models;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagementSystem.Repo.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategories();
        Task<Category> GetCategoryById(int id);
        Task<Category> CreateCategory(Category category);
        Task<bool> UpdateCategory(Category category);
        Task<bool> DeleteCategory(int id);
    }

    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(NewsManagementDBContext context) : base(context)
        {
        }

        public async Task<Category> CreateCategory(Category category)
        {
            try
            {
                await CreateAsync(category);
                return category;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the category.", ex);
            }
        }

        public async Task<bool> DeleteCategory(int id)
        {
            try
            {
                var category = await GetByIdAsync(id);
                if (category == null)
                {
                    return false;
                }

                category.IsActive = false;
                await UpdateAsync(category);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the category.", ex);
            }
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            try
            {
                var categories = await _context.Category
                    .Include(c => c.ParentCategory)
                    .Where(c => c.IsActive == true)
                    .ToListAsync();
                return categories;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving categories.", ex);
            }
        }

        public async Task<Category?> GetCategoryById(int id)
        {
            try
            {
                var category = await _context.Category
                    .Where(c => c.IsActive == true)
                    .Include(c => c.ParentCategory)
                    .FirstOrDefaultAsync(c => c.CategoryId == id);
                return category;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the category.", ex);
            }
        }

        public async Task<bool> UpdateCategory(Category category)
        {
            try
            {
                await UpdateAsync(category);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the category.", ex);
            }
        }
    }
}
