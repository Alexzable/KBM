
using KBMGrpcService.DTOs.User;
using KBMGrpcService.Shared.Helpers;

namespace KBMGrpcService.Services.Interfaces
{
    public interface IUserService
    {
        Task<Guid> CreateAsync(CreateUserDto dto);
        Task<UserDto> GetByIdAsync(Guid id);
        Task<PaginatedList<UserDto>> QueryAsync(int page, int pageSize, string orderBy, bool descending, string? query);
        Task UpdateAsync(UpdateUserDto dto);
        Task DeleteAsync(Guid id);
        Task AssociateAsync(Guid userId, Guid organizationId);
        Task DisassociateAsync(Guid userId, Guid organizationId);
        Task<PaginatedList<UserDto>> QueryForOrganizationAsync(Guid organizationId, int page, int pageSize, string orderBy, bool descending, string? query);
    }
}
