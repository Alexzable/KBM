using KBMHttpService.Shared.Exceptions;
using KBMHttpService.DTOs.Organization;
using Microsoft.AspNetCore.Mvc;
using KBMHttpService.Shared.Helpers;
using KBMHttpService.Services.Interfaces;

namespace KBMHttpService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizationsController(IOrganizationService service, ILogger<OrganizationsController> logger) : ControllerBase
    {
        private readonly IOrganizationService _service = service;
        private readonly ILogger<OrganizationsController> _logger = logger;

        [HttpPost]
        public Task<IActionResult> Create([FromBody] CreateOrganizationDto request)
        {
            return ExceptionUtils.TryCatchAsync(async () =>
            {
                var id = await _service.CreateOrganizationAsync(request);
                return CreatedAtAction(nameof(GetById), new { id }, new ResultId<Guid> { Id = id });
            }, _logger, "Create organization");
        }

        [HttpGet("{id}")]
        public Task<IActionResult> GetById(Guid id)
        {
            return ExceptionUtils.TryCatchAsync(async () =>
            {
                var org = await _service.GetOrganizationByIdAsync(id);
                return Ok(org);
            }, _logger, "Get organization by ID");
        }

        [HttpGet]
        public Task<IActionResult> Query([FromQuery] OrganizationListParamsDto query)
        {
            return ExceptionUtils.TryCatchAsync(async () =>
            {
                var result = await _service.QueryOrganizationsAsync(query);
                return Ok(result);
            }, _logger, "Query organizations");
        }

        [HttpPut("{id}")]
        public Task<IActionResult> Update(Guid id, [FromBody] UpdateOrganizationDto request)
        {
            return ExceptionUtils.TryCatchAsync(async () =>
            {
                request.Id = id;
                await _service.UpdateOrganizationAsync(request);
                return NoContent();
            }, _logger, "Update organization");
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> Delete(Guid id)
        {
            return ExceptionUtils.TryCatchAsync(async () =>
            {
                await _service.DeleteOrganizationAsync(id);
                return NoContent();
            }, _logger, "Delete organization");
        }
    }
}