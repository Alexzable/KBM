using KBMHttpService.Common;
using KBMHttpService.Common.Exceptions;
using KBMHttpService.DTOs.Organization;
using KBMHttpService.Services;
using Microsoft.AspNetCore.Mvc;

namespace KBMHttpService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizationsController(OrganizationService grpcClient, ILogger<OrganizationsController> logger) : ControllerBase
    {
        private readonly OrganizationService _grpcClient = grpcClient;
        private readonly ILogger<OrganizationsController> _logger = logger;

        [HttpPost]
        public Task<IActionResult> Create([FromBody] CreateOrganizationDto request)
        {
            return ExceptionUtils.TryCatchAsync(async () =>
            {
                var id = await _grpcClient.CreateOrganizationAsync(request);
                return CreatedAtAction(nameof(GetById), new { id }, new ResultId<Guid> { Id = id });
            }, _logger, "Create organization");
        }

        [HttpGet("{id}")]
        public Task<IActionResult> GetById(Guid id)
        {
            return ExceptionUtils.TryCatchAsync(async () =>
            {
                var org = await _grpcClient.GetOrganizationByIdAsync(id);
                return Ok(org);
            }, _logger, "Get organization by ID");
        }

        [HttpGet]
        public Task<IActionResult> Query([FromQuery] OrganizationListParamsDto query)
        {
            return ExceptionUtils.TryCatchAsync(async () =>
            {
                var result = await _grpcClient.QueryOrganizationsAsync(query);
                return Ok(result);
            }, _logger, "Query organizations");
        }

        [HttpPut("{id}")]
        public Task<IActionResult> Update(Guid id, [FromBody] UpdateOrganizationDto request)
        {
            return ExceptionUtils.TryCatchAsync(async () =>
            {
                request.Id = id;
                await _grpcClient.UpdateOrganizationAsync(request);
                return NoContent();
            }, _logger, "Update organization");
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> Delete(Guid id)
        {
            return ExceptionUtils.TryCatchAsync(async () =>
            {
                await _grpcClient.DeleteOrganizationAsync(id);
                return NoContent();
            }, _logger, "Delete organization");
        }
    }
}