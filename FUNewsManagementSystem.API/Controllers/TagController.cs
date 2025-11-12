using FUNewsManagementSystem.Repo.DTO;
using FUNewsManagementSystem.Repo.Metadata;
using FUNewsManagementSystem_Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem.API.Controllers
{
    [Route("api/tag")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagService _service;

        public TagController(ITagService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTags()
        {
            var response = await _service.GetAllTags();
            if (response == null || !response.Any())
            {
                return NotFound(ApiResponseBuilder.BuildResponse<IEnumerable<TagDTO>>(
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
        public async Task<IActionResult> GetTagById(int id)
        {
            var response = await _service.GetTagById(id);
            if (response == null)
            {
                return NotFound(ApiResponseBuilder.BuildResponse<TagDTO>(
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
        public async Task<IActionResult> CreateTag([FromBody] CreateTagRequest request)
        {
            var result = await _service.CreateTag(request);
            if (result == null)
            {
                return BadRequest(ApiResponseBuilder.BuildResponse<TagDTO>(
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
        public async Task<IActionResult> UpdateTag(int id, [FromBody] CreateTagRequest request)
        {
            var result = await _service.UpdateTag(id, request);
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
        public async Task<IActionResult> DeleteTag(int id)
        {
            var result = await _service.DeleteTag(id);
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
