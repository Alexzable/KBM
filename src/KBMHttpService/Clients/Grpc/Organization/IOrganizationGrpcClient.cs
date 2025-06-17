using KBMHttpService.API.Features.Organization.Models.Requests;
using KBMHttpService.API.Features.Organization.Models.Responses;

namespace KBMHttpService.Clients.Grpc.Organization
{
    public interface IOrganizationGrpcClient
    {
        Task<Guid> CreateOrganizationAsync(CreateOrganizationRequest request);
        Task<OrganizationResponse> GetOrganizationByIdAsync(Guid id);
        Task<OrganizationsResponse> QueryOrganizationsAsync(OrganizationsRequest request);
        Task UpdateOrganizationAsync(UpdateOrganizationRequest request);
        Task DeleteOrganizationAsync(Guid id);
    }
}
