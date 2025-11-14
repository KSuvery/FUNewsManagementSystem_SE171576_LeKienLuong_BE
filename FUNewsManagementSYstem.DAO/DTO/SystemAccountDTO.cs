using System.ComponentModel.DataAnnotations;

namespace FUNewsManagementSystem.Repo.DTO
{
    public class SystemAccountDTO
    {
        public int AccountId { get; set; }

        public string AccountName { get; set; } = null!;

        public string AccountEmail { get; set; } = null!;

        public int AccountRole { get; set; }
    }

    public class LoginRequest
    {
        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }

    public class LoginResponse
    {
        public string Token { get; set; } = null!;
        public string Role { get; set; } = null!;
    }

    public class RegisterRequest
    {
        [Required]
        public string? AccountName { get; set; }
        [Required]
        public string? AccountEmail { get; set; }
        [Required]
        public string? AccountPassword { get; set; }
    }

    public class RegisterResponse
    {
        public string? AccountName { get; set; }
        public string? AccountEmail { get; set; }
        public string? AccountPassword { get; set; }
    }
}
