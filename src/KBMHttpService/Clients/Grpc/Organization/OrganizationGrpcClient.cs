using AutoMapper;
using KBMHttpService.API.Features.Organization.Models.Requests;
using KBMHttpService.API.Features.Organization.Models.Responses;

namespace KBMHttpService.Clients.Grpc.Organization
{
    public class OrganizationGrpcClient : IOrganizationGrpcClient
    {
        private readonly KBMGrpcService.Grpc.OrganizationService.OrganizationServiceClient _client;
        private readonly IMapper _mapper;

        public OrganizationGrpcClient(KBMGrpcService.Grpc.OrganizationService.OrganizationServiceClient client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }

        public async Task<Guid> CreateOrganizationAsync(CreateOrganizationRequest request)
        {
            var protoReq = _mapper.Map<KBMGrpcService.Grpc.CreateOrganizationRequest>(request);
            var reply = await _client.CreateOrganizationAsync(protoReq);
            return _mapper.Map<CreateOrganizationResponse>(reply).Id;
        }

        public async Task<OrganizationResponse> GetOrganizationByIdAsync(Guid id)
        {
            var reply = await _client.GetOrganizationByIdAsync(new KBMGrpcService.Grpc.GetOrganizationByIdRequest { Id = id.ToString() });
            return _mapper.Map<OrganizationResponse>(reply.Organization);
        }

        public async Task<OrganizationsResponse> QueryOrganizationsAsync(OrganizationsRequest request)
        {
            var protoReq = _mapper.Map<KBMGrpcService.Grpc.QueryOrganizationsRequest>(request);
            var reply = await _client.QueryOrganizationsAsync(protoReq);
            return _mapper.Map<OrganizationsResponse>(reply);
        }

        public async Task UpdateOrganizationAsync(UpdateOrganizationRequest request)
        {
            var protoReq = _mapper.Map<KBMGrpcService.Grpc.UpdateOrganizationRequest>(request);
            await _client.UpdateOrganizationAsync(protoReq);
        }

        public async Task DeleteOrganizationAsync(Guid id)
        {
            await _client.DeleteOrganizationAsync(new KBMGrpcService.Grpc.DeleteOrganizationRequest { Id = id.ToString() });
        }
    }
}
