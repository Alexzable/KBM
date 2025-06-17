using KBMHttpService.API.Base;
using KBMHttpService.API.Features.User.Models.Requests;
using KBMHttpService.API.Features.User.Models.Responses;
using KBMHttpService.Clients.Grpc.User;
using Microsoft.AspNetCore.Mvc;

namespace KBMHttpService.API.Features.User.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IUserGrpcClient _grpcClient;

        public UsersController(IUserGrpcClient grpcClient)
        {
            _grpcClient = grpcClient;
        }

        [HttpPost]
        public async Task<ActionResult<CreateUserResponse>> Create([FromBody] CreateUserRequest request)
        {
            var id = await _grpcClient.CreateUserAsync(request);
            return CreatedAtAction(nameof(GetById), new { id }, new CreateUserResponse { Id = id });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetById(Guid id)
        {
            var user = await _grpcClient.GetUserByIdAsync(id);
            return Ok(user);
        }

        [HttpGet]
        public async Task<ActionResult<UsersResponse>> Query([FromQuery] UsersRequest query)
        {
            var result = await _grpcClient.QueryUsersAsync(query);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserRequest request)
        {
            request.Id = id;
            await _grpcClient.UpdateUserAsync(request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _grpcClient.DeleteUserAsync(id);
            return NoContent();
        }

        [HttpPost("{userId}/organizations/{orgId}")]
        public async Task<IActionResult> Associate(Guid userId, Guid orgId)
        {
            await _grpcClient.AssociateUserAsync(new AssociateUserRequest { UserId = userId, OrganizationId = orgId });
            return NoContent();
        }

        [HttpDelete("{userId}/organizations/{orgId}")]
        public async Task<IActionResult> Disassociate(Guid userId, Guid orgId)
        {
            await _grpcClient.DisassociateUserAsync(new AssociateUserRequest { UserId = userId, OrganizationId = orgId });
            return NoContent();
        }

        [HttpGet("organization/{orgId}")]
        public async Task<ActionResult<UsersResponse>> QueryForOrganization(Guid orgId, [FromQuery] UsersForOrganizationRequest query)
        {
            query.OrganizationId = orgId;
            var result = await _grpcClient.QueryUsersForOrganizationAsync(query);
            return Ok(result);
        }
    }
}
