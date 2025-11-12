using FUNewsManagementSystem.Repo.DTO;
using FUNewsManagementSystem.Repo.Metadata;
using FUNewsManagementSystem_Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem.API.Controllers
{
    [Route("api/category")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var response = await _service.GetAllCategories();
            if (response == null || !response.Any())
            {
                return NotFound(ApiResponseBuilder.BuildResponse<IEnumerable<CategoryDTO>>(
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
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var response = await _service.GetCategoryById(id);
            if (response == null)
            {
                return NotFound(ApiResponseBuilder.BuildResponse<CategoryDTO>(
                    StatusCodes.Status404NotFound,
                    "Failure.",
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
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            var result = await _service.CreateCategory(request);
            if (result == null)
            {
                return BadRequest(ApiResponseBuilder.BuildResponse<CategoryDTO>(
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
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CreateCategoryRequest request)
        {
            var result = await _service.UpdateCategory(id, request);
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
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _service.DeleteCategory(id);
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
