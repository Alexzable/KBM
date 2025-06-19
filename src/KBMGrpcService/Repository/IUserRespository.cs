using KBMGrpcService.Entities;
using KBMGrpcService.Shared.Helpers;

namespace KBMGrpcService.Repository
{
    public interface IUserRepository
    {
        Task<Guid> CreateAsync(User user);
        Task<User?> GetByIdAsync(Guid id);
        Task<PaginatedList<User>> QueryAsync(int page, int pageSize, string orderBy, bool descending, string? query);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task AssociateAsync(Guid userId, Guid organizationId);
        Task DisassociateAsync(Guid userId, Guid organizationId);
        Task<PaginatedList<User>> QueryForOrganizationAsync(Guid organizationId, int page, int pageSize, string orderBy, bool descending, string? query);
    }

}
