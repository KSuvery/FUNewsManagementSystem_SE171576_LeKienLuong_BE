using System.ComponentModel.DataAnnotations;

namespace FUNewsManagementSystem.Repo.DTO
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;

        public string? CategoryDescription { get; set; }

        public string? ParentCategory { get; set; }

        public bool IsActive { get; set; }
    }

    public class CreateCategoryRequest
    {
        [Required]
        public string CategoryName { get; set; } = null!;
        [Required]
        public string? CategoryDescription { get; set; }
        [Required]
        public int ParentCategoryId { get; set; }
        [Required]
        public bool IsActive { get; set; } = true;
    }
}
