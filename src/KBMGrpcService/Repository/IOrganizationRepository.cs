using KBMGrpcService.Entities;
using KBMGrpcService.Shared.Helpers;

namespace KBMGrpcService.Repository
{
    public interface IOrganizationRepository
    {
        Task<Guid> CreateAsync(Organization org);
        Task<Organization?> GetByIdAsync(Guid id);
        Task<PaginatedList<Organization>> QueryAsync(int page, int pageSize, string orderBy, bool descending, string? query);
        Task UpdateAsync(Organization org);
        Task DeleteAsync(Organization org);
    }
}
