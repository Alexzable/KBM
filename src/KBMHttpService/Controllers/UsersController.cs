using KBMHttpService.Common;
using KBMHttpService.Common.Exceptions;
using KBMHttpService.DTOs.User;
using KBMHttpService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KBMHttpService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(IUserService grpcClient, ILogger<UsersController> logger) : ControllerBase
    {
        private readonly IUserService _grpcClient = grpcClient;
        private readonly ILogger<UsersController> _logger = logger;

        [HttpPost]
        public Task<IActionResult> Create([FromBody] CreateUserDto request)
        {
            return ExceptionUtils.TryCatchAsync(async () =>
            {
                var id = await _grpcClient.CreateUserAsync(request);
                return CreatedAtAction(nameof(GetById), new { id }, new ResultId<Guid> { Id = id });
            }, _logger, "Create user");
        }

        [HttpGet("{id}")]
        public Task<IActionResult> GetById(Guid id)
        {
            return ExceptionUtils.TryCatchAsync(async () =>
            {
                var user = await _grpcClient.GetUserByIdAsync(id);
                return Ok(user);
            }, _logger, "Get user by ID");
        }

        [HttpGet]
        public Task<IActionResult> Query([FromQuery] UserListParamsDto query)
        {
            return ExceptionUtils.TryCatchAsync(async () =>
            {
                var result = await _grpcClient.QueryUsersAsync(query);
                return Ok(result);
            }, _logger, "Query users");
        }

        [HttpPut("{id}")]
        public Task<IActionResult> Update(Guid id, [FromBody] UpdateUserDto request)
        {
            return ExceptionUtils.TryCatchAsync(async () =>
            {
                request.Id = id;
                await _grpcClient.UpdateUserAsync(request);
                return NoContent();
            }, _logger, "Update user");
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> Delete(Guid id)
        {
            return ExceptionUtils.TryCatchAsync(async () =>
            {
                await _grpcClient.DeleteUserAsync(id);
                return NoContent();
            }, _logger, "Delete user");
        }

        [HttpPost("{userId}/organizations/{orgId}")]
        public Task<IActionResult> Associate(Guid userId, Guid orgId)
        {
            return ExceptionUtils.TryCatchAsync(async () =>
            {
                await _grpcClient.AssociateUserAsync(new AssociateUserDto { UserId = userId, OrganizationId = orgId });
                return NoContent();
            }, _logger, "Associate user");
        }

        [HttpDelete("{userId}/organizations/{orgId}")]
        public Task<IActionResult> Disassociate(Guid userId, Guid orgId)
        {
            return ExceptionUtils.TryCatchAsync(async () =>
            {
                await _grpcClient.DisassociateUserAsync(new AssociateUserDto { UserId = userId, OrganizationId = orgId });
                return NoContent();
            }, _logger, "Disassociate user");
        }

        [HttpGet("organization/{orgId}")]
        public Task<IActionResult> QueryForOrganization(Guid orgId, [FromQuery] UsersForOrganizationDto query)
        {
            return ExceptionUtils.TryCatchAsync(async () =>
            {
                query.OrganizationId = orgId;
                var result = await _grpcClient.QueryUsersForOrganizationAsync(query);
                return Ok(result);
            }, _logger, "Query users for org");
        }
    }
}
