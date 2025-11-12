using FUNewsManagementSystem.Repo.DTO;
using FUNewsManagementSystem.Repo.Mapper;
using FUNewsManagementSystem.Repo.Repositories;

namespace FUNewsManagementSystem_Service.Services
{
    public interface ICategoryService
    {
        Task<CategoryDTO> CreateCategory(CreateCategoryRequest request);
        Task<bool> UpdateCategory(int id, CreateCategoryRequest request);
        Task<bool> DeleteCategory(int id);
        Task<IEnumerable<CategoryDTO>> GetAllCategories();
        Task<CategoryDTO?> GetCategoryById(int id);
    }

    public class CategoryService : ICategoryService
    {
        private readonly IMapperlyMapper _mapper;
        private readonly ICategoryRepository _repo;

        public CategoryService(IMapperlyMapper mapper, ICategoryRepository repo)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<CategoryDTO> CreateCategory(CreateCategoryRequest request)
        {
            try
            {
                var category = _mapper.CreateMapToCategory(request);
                await _repo.CreateCategory(category);

                return _mapper.MapToCategoryDTO(category);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteCategory(int id)
        {
            try
            {
                return await _repo.DeleteCategory(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategories()
        {
            try
            {
                var categories = await _repo.GetAllCategories();
                return categories.Select(c => _mapper.MapToCategoryDTO(c));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CategoryDTO?> GetCategoryById(int id)
        {
            try
            {
                var category = await _repo.GetCategoryById(id);
                if (category == null) return null;

                return _mapper.MapToCategoryDTO(category);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateCategory(int id, CreateCategoryRequest request)
        {
            try
            {
                var existingCategory = await _repo.GetCategoryById(id);
                if (existingCategory == null) return false;

                var category = _mapper.CreateMapToCategory(request);
                category.CategoryId = id;

                return await _repo.UpdateCategory(category);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
