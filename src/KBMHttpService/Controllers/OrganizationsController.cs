using KBMHttpService.Common;
using KBMHttpService.Common.Exceptions;
using KBMHttpService.DTOs.Organization;
using KBMHttpService.Services;
using Microsoft.AspNetCore.Mvc;

namespace KBMHttpService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizationsController : ControllerBase
    {
        private readonly OrganizationService _grpcClient;
        private readonly ILogger<OrganizationsController> _logger;

        public OrganizationsController(OrganizationService grpcClient, ILogger<OrganizationsController> logger)
        {
            _grpcClient = grpcClient;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrganizationDto request)
        {
            try
            {
                var id = await _grpcClient.CreateOrganizationAsync(request);
                return CreatedAtAction(nameof(GetById), new { id }, new ResultId<Guid> { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create organization failed");
                return ExceptionUtils.HandleException(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var org = await _grpcClient.GetOrganizationByIdAsync(id);
                return Ok(org);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get organization by ID failed");
                return ExceptionUtils.HandleException(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Query([FromQuery] OrganizationListParamsDto query)
        {
            try
            {
                var result = await _grpcClient.QueryOrganizationsAsync(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Query organizations failed");
                return ExceptionUtils.HandleException(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOrganizationDto request)
        {
            try
            {
                request.Id = id;
                await _grpcClient.UpdateOrganizationAsync(request);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update organization failed");
                return ExceptionUtils.HandleException(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _grpcClient.DeleteOrganizationAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete organization failed");
                return ExceptionUtils.HandleException(ex);
            }
        }

    }
}