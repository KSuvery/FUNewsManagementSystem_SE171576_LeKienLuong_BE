using FUNewsManagementSystem.Repo.DTO;
using FUNewsManagementSystem.Repo.Mapper;
using FUNewsManagementSystem.Repo.Models;
using FUNewsManagementSystem.Repo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FUNewsManagementSystem_Service.Services
{
    public interface ISystemAccountService
    {
        public Task<LoginResponse> Login(LoginRequest request);
        public Task<RegisterResponse> Register(RegisterRequest request);
        public Task<SystemAccountDTO> GetProfile();
    }
    public class SystemAccountService : ISystemAccountService
    {
        private readonly ISystemAccountRepository _repo;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapperlyMapper _mapper;

        public SystemAccountService(ISystemAccountRepository repo, IConfiguration config,
            IHttpContextAccessor contextAccessor, IMapperlyMapper mapper)
        {
            _repo = repo;
            _config = config;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            //appsettings admin check
            var adminEmail = _config["AdminCredentials:AccountEmail"];
            var adminPassword = _config["AdminCredentials:AccountPassword"];
            var adminId = _config["AdminCredentials:AccountId"];
            var adminName = _config["AdminCredentials:AccountName"];
            var adminRole = _config["AdminCredentials:AccountRole"];

            if (request.Email == adminEmail &&
                request.Password == adminPassword)
            {
                var adminRoles = new List<string> { adminRole.ToString() };
                var adminToken = GenerateJSONWebToken(
                    adminId.ToString(),
                    adminEmail,
                    adminRoles
                );
                return new LoginResponse
                {
                    Token = adminToken,
                    Role = "0"
                };
            }

            //database account check
            var account = await _repo.GetSystemAccountByEmail(request.Email);
            if (account == null && BCrypt.Net.BCrypt.Verify(request.Password, account.AccountPassword))
            {
                return null;
            }
            var roles = new List<string>();
            if (account.AccountRole != 0)
            {
                roles.Add(account.AccountRole.ToString());
            }
            var token = GenerateJSONWebToken(
                account.AccountId.ToString(),
                account.AccountEmail ?? string.Empty,
                roles
            );
            var refreshToken = GenerateRefreshToken();

            return new LoginResponse
            {
                Token = token,
                Role = account.AccountRole != 0 ? account.AccountRole.ToString() : "0"
            };
        }

        public async Task<RegisterResponse> Register(RegisterRequest request)
        {
            var existingAccount = await _repo.GetSystemAccountByEmail(request.AccountEmail);
            try
            {
                if (existingAccount != null)
                {
                    return null; // Account with the same email already exists
                }
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.AccountPassword);
                var newAccount = new SystemAccount
                {
                    AccountName = request.AccountName,
                    AccountEmail = request.AccountEmail,
                    AccountPassword = hashedPassword,
                    AccountRole = 2 // Default role for regular users
                };
                await _repo.CreateSystemAccount(newAccount);
                var response = new RegisterResponse
                {
                    AccountName = newAccount.AccountName,
                    AccountEmail = newAccount.AccountEmail,
                    AccountPassword = null // Do not return password
                };
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<SystemAccountDTO> GetProfile()
        {
            try
            {
                var userIdClaim = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var profile = await _repo.GetProfile(int.Parse(userIdClaim));

                var dto = _mapper.MapToSystemAccountDTO(profile);
                return dto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string GenerateJSONWebToken(string memberId, string email, IList<string> roles = null)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var tokenExpirationMinutes = int.Parse(_config["Jwt:TokenExpirationMinutes"] ?? "60");
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, memberId.ToString()),
            new Claim(ClaimTypes.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

            if (roles != null && roles.Any())
            {
                foreach (var role in roles)
                {
                    // Convert numeric roles to string roles
                    string stringRole;
                    if (int.TryParse(role, out int roleValue))
                    {
                        stringRole = roleValue switch
                        {
                            3 => "Admin",
                            2 => "Lecterer",
                            1 => "Staff",
                            _ => "Unknown"
                        };
                    }
                    else
                    {
                        stringRole = role;
                    }
                    claims.Add(new Claim(ClaimTypes.Role, stringRole));
                }
            }

            var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(tokenExpirationMinutes),
            signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }

        private string GenerateRefreshToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var tokenExpirationMinutes = int.Parse(_config["Jwt:TokenExpirationMinutes"] ?? "60");
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7), // Refresh tokens typically last longer than access tokens
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
