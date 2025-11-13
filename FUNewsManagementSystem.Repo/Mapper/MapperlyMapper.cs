using FUNewsManagementSystem.Repo.DTO;
using FUNewsManagementSystem.Repo.Models;
using Riok.Mapperly.Abstractions;

namespace FUNewsManagementSystem.Repo.Mapper
{
    public interface IMapperlyMapper
    {
        NewsArticle MapToNewsArticle(NewsArticleDTO dto);
        NewsArticle MapCreateNewsArticleRequestToNewsArticle(CreateNewsArticleRequest request);
        NewsArticleDTO MapToNewsArticleDTO(NewsArticle newsArticle);
        NewsArticleDTO MapToNewsArticleDTO(IEnumerable<NewsArticle> newsArticles);
        NewsArticleDTO CreateRequestMapToNewsArticleDTO(CreateNewsArticleRequest request);
        NewsArticleDTO UpdateRequestMapToNewsArticleDTO(UpdateNewsArticleRequest request);

        Tag MapToTag(TagDTO dto);
        TagDTO MapToTagDTO(Tag tag);
        Tag CreateMapToTag(CreateTagRequest request);

        Category MapToCategory(CategoryDTO dto);
        CategoryDTO MapToCategoryDTO(Category category);
        Category CreateMapToCategory(CreateCategoryRequest request);

        SystemAccount MapToSystemAccount(RegisterRequest request);
        RegisterResponse MapToRegisterResponse(SystemAccount account);
        SystemAccountDTO MapToSystemAccountDTO(SystemAccount account);
    }

    [Mapper]
    public partial class MapperlyMapper : IMapperlyMapper
    {

        public partial NewsArticle MapToNewsArticle(NewsArticleDTO dto);
        public partial NewsArticle MapCreateNewsArticleRequestToNewsArticle(CreateNewsArticleRequest request);
        [MapProperty(nameof(NewsArticle.CreatedBy.AccountName), nameof(NewsArticleDTO.CreatedBy))]
        [MapProperty(nameof(NewsArticle.UpdatedBy.AccountName), nameof(NewsArticleDTO.UpdatedBy))]
        [MapProperty(nameof(NewsArticle.Category.CategoryName), nameof(NewsArticleDTO.Category))]
        public partial NewsArticleDTO MapToNewsArticleDTO(NewsArticle newsArticle);
        public partial NewsArticleDTO MapToNewsArticleDTO(IEnumerable<NewsArticle> newsArticles);
        public partial NewsArticleDTO CreateRequestMapToNewsArticleDTO(CreateNewsArticleRequest request);
        public partial NewsArticleDTO UpdateRequestMapToNewsArticleDTO(UpdateNewsArticleRequest request);


        public partial Tag MapToTag(TagDTO dto);
        public partial Tag CreateMapToTag(CreateTagRequest request);
        [MapProperty(nameof(Tag.TagName), nameof(TagDTO.TagName))]
        public partial TagDTO MapToTagDTO(Tag tag);
        

        public partial Category MapToCategory(CategoryDTO dto);
        public partial Category CreateMapToCategory(CreateCategoryRequest request);
        [MapProperty(nameof(Category.CategoryName), nameof(CategoryDTO.CategoryName))]
        public partial CategoryDTO MapToCategoryDTO(Category category);
        

        public partial SystemAccount MapToSystemAccount(RegisterRequest request);
        public partial RegisterResponse MapToRegisterResponse(SystemAccount account);
        [MapProperty(nameof(SystemAccount.AccountName), nameof(SystemAccountDTO.AccountName))]
        [MapProperty(nameof(SystemAccount.AccountEmail), nameof(SystemAccountDTO.AccountEmail))]
        [MapProperty(nameof(SystemAccount.AccountRole), nameof(SystemAccountDTO.AccountRole))]
        [MapProperty(nameof(SystemAccount.AccountId), nameof(SystemAccountDTO.AccountId))]
        public partial SystemAccountDTO MapToSystemAccountDTO(SystemAccount account);
    }
}
