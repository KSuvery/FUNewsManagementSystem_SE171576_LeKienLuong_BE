using System.ComponentModel.DataAnnotations;

namespace FUNewsManagementSystem.Repo.DTO
{
    public class NewsArticleDTO
    {
        public int NewsArticleId { get; set; }

        public string NewsTitle { get; set; } = null!;

        public string? Headline { get; set; }

        public DateTime CreatedDate { get; set; }

        public string? NewsContent { get; set; }

        public string? NewsSource { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string? Category { get; set; }

        public List<TagDTO> Tags { get; set; } = new List<TagDTO>();

        public bool NewsStatus { get; set; }
    }

    public class CreateNewsArticleRequest
    {
        [Required]
        public string NewsTitle { get; set; } = null!;
        [Required]
        public string? Headline { get; set; }
        [Required]
        public string? NewsContent { get; set; }
        [Required]
        public string? NewsSource { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public ICollection<string>? Tags { get; set; }
    }

    public class UpdateNewsArticleRequest
    {
        [Required]
        public string NewsTitle { get; set; } = null!;
        [Required]
        public string? Headline { get; set; }
        [Required]
        public string? NewsContent { get; set; }
        [Required]
        public string? NewsSource { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public bool NewsStatus { get; set; }
        [Required]
        public ICollection<string>? Tags { get; set; }
    }
}
