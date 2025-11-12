using FUNewsManagementSystem.Repo.DTO;
using FUNewsManagementSystem.Repo.Metadata;
using FUNewsManagementSystem_Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem.API.Controllers
{
    [Route("api/article")]
    [ApiController]
    public class NewsArticleController : ControllerBase
    {
        private readonly INewsArticleService _newsService;

        public NewsArticleController(INewsArticleService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNewsArticle()
        {
            var response = await _newsService.GetAllNewsArticle();
            if (response == null || !response.Any())
            {
                return NotFound(ApiResponseBuilder.BuildResponse<IEnumerable<NewsArticleDTO>>(
                    StatusCodes.Status404NotFound,
                    "None found in database.",
                    null
                ));
            }

            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Success.",
                response
            ));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNewsArticleById(int id)
        {
            var response = await _newsService.GetNewsArticleById(id);
            if (response == null)
            {
                return NotFound(ApiResponseBuilder.BuildResponse<NewsArticleDTO>(
                    StatusCodes.Status404NotFound,
                    "None found in database.",
                    null
                ));
            }

            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Success.",
                response
            ));
        }

        [HttpPost("create")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> CreateNewsArticle([FromBody] CreateNewsArticleRequest request)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)
                      ?? User.FindFirst("sub");

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized(ApiResponseBuilder.BuildResponse<string>(
                    StatusCodes.Status401Unauthorized,
                    "Invalid token.",
                    null
                ));
            }

            var result = await _newsService.CreateNewsArticle(userId, request);

            if (result == null)
            {
                return BadRequest(ApiResponseBuilder.BuildResponse<NewsArticleDTO>(
                    StatusCodes.Status400BadRequest,
                    "Failure.",
                    null
                ));
            }

            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Success.",
                result
            ));
        }

        [HttpPut("update/{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> UpdateNewsArticle(int id, [FromBody] UpdateNewsArticleRequest request)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)
                      ?? User.FindFirst("sub");

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized(ApiResponseBuilder.BuildResponse<string>(
                    StatusCodes.Status401Unauthorized,
                    "Invalid token.",
                    null
                ));
            }

            var result = await _newsService.UpdateNewsArticle(userId, id, request);

            if (!result)
            {
                return BadRequest(ApiResponseBuilder.BuildResponse<bool>(
                    StatusCodes.Status400BadRequest,
                    "Failure.",
                    false
                ));
            }

            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Success.",
                true
            ));
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> DeleteNewsArticle(int id)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)
                      ?? User.FindFirst("sub");

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized(ApiResponseBuilder.BuildResponse<string>(
                    StatusCodes.Status401Unauthorized,
                    "Invalid token.",
                    null
                ));
            }

            var result = await _newsService.DeleteNewsArticle(id);

            if (!result)
            {
                return BadRequest(ApiResponseBuilder.BuildResponse<bool>(
                    StatusCodes.Status400BadRequest,
                    "Failure.",
                    false
                ));
            }

            return Ok(ApiResponseBuilder.BuildResponse(
                StatusCodes.Status200OK,
                "Success.",
                true
            ));
        }
    }
}
