using KBMHttpService.DTOs.User;

namespace KBMHttpService.Services.Interfaces
{
    public interface IUserService
    {
        Task<Guid> CreateUserAsync(CreateUserDto request);
        Task<UserDto> GetUserByIdAsync(Guid id);
        Task<UserListDto> QueryUsersAsync(UserListParamsDto request);
        Task UpdateUserAsync(UpdateUserDto request);
        Task DeleteUserAsync(Guid id);
        Task AssociateUserAsync(AssociateUserDto request);
        Task DisassociateUserAsync(AssociateUserDto request);
        Task<UserListDto> QueryUsersForOrganizationAsync(UsersForOrganizationDto request);
    }
}
