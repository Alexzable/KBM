using KBMHttpService.API.Base;
using KBMHttpService.API.Features.Organization.Models.Requests;
using KBMHttpService.API.Features.Organization.Models.Responses;
using KBMHttpService.Clients.Grpc.Organization;
using Microsoft.AspNetCore.Mvc;

namespace KBMHttpService.API.Features.Organization.Controllers
{
    public class OrganizationsController : BaseApiController
    {
        private readonly IOrganizationGrpcClient _grpcClient;

        public OrganizationsController(IOrganizationGrpcClient grpcClient)
        {
            _grpcClient = grpcClient;
        }

        [HttpPost]
        public async Task<ActionResult<CreateOrganizationResponse>> Create([FromBody] CreateOrganizationRequest request)
        {
            var id = await _grpcClient.CreateOrganizationAsync(request);
            return CreatedAtAction(nameof(GetById), new { id }, new CreateOrganizationResponse { Id = id });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrganizationResponse>> GetById(Guid id)
        {
            var org = await _grpcClient.GetOrganizationByIdAsync(id);
            return Ok(org);
        }

        [HttpGet]
        public async Task<ActionResult<OrganizationsResponse>> Query([FromQuery] OrganizationsRequest query)
        {
            var result = await _grpcClient.QueryOrganizationsAsync(query);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOrganizationRequest request)
        {
            request.Id = id;
            await _grpcClient.UpdateOrganizationAsync(request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _grpcClient.DeleteOrganizationAsync(id);
            return NoContent();
        }

    }
}