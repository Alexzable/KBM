using KBMHttpService.API.Base;
using KBMHttpService.API.Features.User.Models.Requests;
using KBMHttpService.API.Features.User.Models.Responses;
using KBMHttpService.Clients.Grpc.User;
using KBMHttpService.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace KBMHttpService.API.Features.User.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IUserGrpcClient _grpcClient;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserGrpcClient grpcClient, ILogger<UsersController> logger)
        {
            _grpcClient = grpcClient;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            try
            {
                var id = await _grpcClient.CreateUserAsync(request);
                return CreatedAtAction(nameof(GetById), new { id }, new CreateUserResponse { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create user failed");
                return ExceptionUtils.HandleException(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var user = await _grpcClient.GetUserByIdAsync(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get user by ID failed");
                return ExceptionUtils.HandleException(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Query([FromQuery] UsersRequest query)
        {
            try
            {
                var result = await _grpcClient.QueryUsersAsync(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Query users failed");
                return ExceptionUtils.HandleException(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserRequest request)
        {
            try
            {
                request.Id = id;
                await _grpcClient.UpdateUserAsync(request);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update user failed");
                return ExceptionUtils.HandleException(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _grpcClient.DeleteUserAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete user failed");
                return ExceptionUtils.HandleException(ex);
            }
        }

        [HttpPost("{userId}/organizations/{orgId}")]
        public async Task<IActionResult> Associate(Guid userId, Guid orgId)
        {
            try
            {
                await _grpcClient.AssociateUserAsync(new AssociateUserRequest { UserId = userId, OrganizationId = orgId });
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Associate user failed");
                return ExceptionUtils.HandleException(ex);
            }
        }

        [HttpDelete("{userId}/organizations/{orgId}")]
        public async Task<IActionResult> Disassociate(Guid userId, Guid orgId)
        {
            try
            {
                await _grpcClient.DisassociateUserAsync(new AssociateUserRequest { UserId = userId, OrganizationId = orgId });
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Disassociate user failed");
                return ExceptionUtils.HandleException(ex);
            }
        }

        [HttpGet("organization/{orgId}")]
        public async Task<IActionResult> QueryForOrganization(Guid orgId, [FromQuery] UsersForOrganizationRequest query)
        {
            try
            {
                query.OrganizationId = orgId;
                var result = await _grpcClient.QueryUsersForOrganizationAsync(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Query users for org failed");
                return ExceptionUtils.HandleException(ex);
            }
        }
    }
}
