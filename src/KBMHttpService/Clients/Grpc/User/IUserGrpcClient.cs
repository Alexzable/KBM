using KBMHttpService.API.Features.User.Models.Requests;
using KBMHttpService.API.Features.User.Models.Responses;

namespace KBMHttpService.Clients.Grpc.User
{
    public interface IUserGrpcClient
    {
        Task<Guid> CreateUserAsync(CreateUserRequest request);
        Task<UserResponse> GetUserByIdAsync(Guid id);
        Task<UsersResponse> QueryUsersAsync(UsersRequest request);
        Task UpdateUserAsync(UpdateUserRequest request);
        Task DeleteUserAsync(Guid id);
        Task AssociateUserAsync(AssociateUserRequest request);
        Task DisassociateUserAsync(AssociateUserRequest request);
        Task<UsersResponse> QueryUsersForOrganizationAsync(UsersForOrganizationRequest request);
    }
}
