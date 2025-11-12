using System.ComponentModel.DataAnnotations;

namespace FUNewsManagementSystem.Repo.DTO
{
    public class TagDTO
    {
        public int TagId { get; set; }

        public string TagName { get; set; } = null!;

        public string? Note { get; set; }
    }

    public class CreateTagRequest
    {
        [Required]
        public string TagName { get; set; } = null!;
        [Required]
        public string? Note { get; set; }
    }
}
