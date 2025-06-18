using AutoMapper;
using Grpc.Core;
using KBMHttpService.API.Features.Organization.Models.Requests;
using KBMHttpService.API.Features.Organization.Models.Responses;
using KBMHttpService.Common.Exceptions;

namespace KBMHttpService.Clients.Grpc.Organization
{
    public class OrganizationGrpcClient : IOrganizationGrpcClient
    {
        private readonly KBMGrpcService.Grpc.OrganizationService.OrganizationServiceClient _client;
        private readonly IMapper _mapper;
        private readonly ILogger<OrganizationGrpcClient> _logger;

        public OrganizationGrpcClient(
            KBMGrpcService.Grpc.OrganizationService.OrganizationServiceClient client,
            IMapper mapper,
            ILogger<OrganizationGrpcClient> logger)
        {
            _client = client;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Guid> CreateOrganizationAsync(CreateOrganizationRequest request)
        {
            var protoReq = ExceptionUtils.ExecuteMapping(() => _mapper.Map<KBMGrpcService.Grpc.CreateOrganizationRequest>(request), _logger);
            try
            {
                var reply = await _client.CreateOrganizationAsync(protoReq);
                return _mapper.Map<CreateOrganizationResponse>(reply).Id;
            }
            catch (RpcException ex)
            {
                throw new ExternalServiceException("Failed to create organization.", ex);
            }
        }

        public async Task<OrganizationResponse> GetOrganizationByIdAsync(Guid id)
        {
            try
            {
                var reply = await _client.GetOrganizationByIdAsync(new KBMGrpcService.Grpc.GetOrganizationByIdRequest { Id = id.ToString() });
                return _mapper.Map<OrganizationResponse>(reply.Organization);
            }
            catch (RpcException ex)
            {
                throw new ExternalServiceException("Failed to get organization.", ex);
            }
        }

        public async Task<OrganizationsResponse> QueryOrganizationsAsync(OrganizationsRequest request)
        {
            var protoReq = ExceptionUtils.ExecuteMapping(() => _mapper.Map<KBMGrpcService.Grpc.QueryOrganizationsRequest>(request), _logger);
            try
            {
                var reply = await _client.QueryOrganizationsAsync(protoReq);
                return _mapper.Map<OrganizationsResponse>(reply);
            }
            catch (RpcException ex)
            {
                throw new ExternalServiceException("Organization query failed.", ex);
            }
        }

        public async Task UpdateOrganizationAsync(UpdateOrganizationRequest request)
        {
            var protoReq = ExceptionUtils.ExecuteMapping(() => _mapper.Map<KBMGrpcService.Grpc.UpdateOrganizationRequest>(request), _logger);
            try
            {
                await _client.UpdateOrganizationAsync(protoReq);
            }
            catch (RpcException ex)
            {
                throw new ExternalServiceException("Update failed.", ex);
            }
        }

        public async Task DeleteOrganizationAsync(Guid id)
        {
            try
            {
                await _client.DeleteOrganizationAsync(new KBMGrpcService.Grpc.DeleteOrganizationRequest { Id = id.ToString() });
            }
            catch (RpcException ex)
            {
                throw new ExternalServiceException("Delete failed.", ex);
            }
        }
    }
}
