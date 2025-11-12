using FUNewsManagementSystem.Repo.DTO;
using FUNewsManagementSystem.Repo.Metadata;
using FUNewsManagementSystem_Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem.API.Controllers
{
    [Route("api/home")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ISystemAccountService _sysAccService;

        public HomeController(ISystemAccountService sysAccService)
        {
            _sysAccService = sysAccService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            try
            {
                var response = await _sysAccService.Login(request);
                if (response == null)
                {
                    return Unauthorized(ApiResponseBuilder.BuildResponse<LoginResponse>(
                        StatusCodes.Status401Unauthorized,
                        "Invalid email or password.",
                        null
                    ));
                }

                return Ok(ApiResponseBuilder.BuildResponse(
                    StatusCodes.Status200OK,
                    "Success.",
                    response
                ));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponseBuilder.BuildResponse<LoginResponse>(
                        StatusCodes.Status500InternalServerError,
                        "An unexpected error occurred while processing the request.",
                        null
                    ));
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var response = await _sysAccService.Register(request);
                if (response == null)
                {
                    return BadRequest(ApiResponseBuilder.BuildResponse<RegisterResponse>(
                            StatusCodes.Status400BadRequest,
                            "Email is already used.",
                            null
                        ));
                }

                return Ok(ApiResponseBuilder.BuildResponse(
                    StatusCodes.Status200OK,
                    "Success.",
                    response
                ));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponseBuilder.BuildResponse<RegisterResponse>(
                        StatusCodes.Status500InternalServerError,
                        "An unexpected error occurred while processing the request.",
                        null
                    ));
            }
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var response = await _sysAccService.GetProfile();
                if (response == null)
                {
                    return BadRequest(ApiResponseBuilder.BuildResponse<RegisterResponse>(
                            StatusCodes.Status400BadRequest,
                            "Invalid token.",
                            null
                        ));
                }

                return Ok(ApiResponseBuilder.BuildResponse(
                    StatusCodes.Status200OK,
                    "Success.",
                    response
                ));

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponseBuilder.BuildResponse<RegisterResponse>(
                        StatusCodes.Status500InternalServerError,
                        "An unexpected error occurred while processing the request.",
                        null
                    ));
            }
        }
    }
}
