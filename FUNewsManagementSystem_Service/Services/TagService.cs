using FUNewsManagementSystem.Repo.DTO;
using FUNewsManagementSystem.Repo.Mapper;
using FUNewsManagementSystem.Repo.Repositories;

namespace FUNewsManagementSystem_Service.Services
{
    public interface ITagService
    {
        Task<TagDTO> CreateTag(CreateTagRequest request);
        Task<bool> UpdateTag(int id, CreateTagRequest request);
        Task<bool> DeleteTag(int id);
        Task<IEnumerable<TagDTO>> GetAllTags();
        Task<TagDTO?> GetTagById(int id);
    }
    public class TagService : ITagService
    {
        private readonly IMapperlyMapper _mapper;
        private readonly ITagRepository _repo;

        public TagService(ITagRepository repo, IMapperlyMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<TagDTO> CreateTag(CreateTagRequest request)
        {
            try
            {
                var tag = _mapper.CreateMapToTag(request);
                await _repo.CreateTag(tag);

                return _mapper.MapToTagDTO(tag);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteTag(int id)
        {
            try
            {
                await _repo.DeleteTag(id);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<TagDTO>> GetAllTags()
        {
            try
            {
                var tags = await _repo.GetAllTags();
                return tags.Select(t => _mapper.MapToTagDTO(t));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TagDTO?> GetTagById(int id)
        {
            try
            {
                var tag = await _repo.GetTagById(id);
                return _mapper.MapToTagDTO(tag);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateTag(int id, CreateTagRequest request)
        {
            try
            {
                var tag = _mapper.CreateMapToTag(request);
                await _repo.UpdateTag(tag);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
