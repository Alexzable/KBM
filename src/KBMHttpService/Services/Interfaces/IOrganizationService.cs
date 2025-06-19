using KBMHttpService.DTOs.Organization;

namespace KBMHttpService.Services.Interfaces
{
    public interface IOrganizationService
    {
        Task<Guid> CreateOrganizationAsync(CreateOrganizationDto request);
        Task<OrganizationDto> GetOrganizationByIdAsync(Guid id);
        Task<OrganizationListDto> QueryOrganizationsAsync(OrganizationListParamsDto request);
        Task UpdateOrganizationAsync(UpdateOrganizationDto request);
        Task DeleteOrganizationAsync(Guid id);
    }
}
