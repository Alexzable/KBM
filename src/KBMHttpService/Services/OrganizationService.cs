using AutoMapper;
using Grpc.Core;
using KBMHttpService.Common;
using KBMHttpService.Common.Exceptions;
using KBMHttpService.DTOs.Organization;
using KBMHttpService.Services.Interfaces;

namespace KBMHttpService.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly KBMGrpcService.Grpc.OrganizationService.OrganizationServiceClient _client;
        private readonly IMapper _mapper;
        private readonly ILogger<OrganizationService> _logger;

        public OrganizationService(
            KBMGrpcService.Grpc.OrganizationService.OrganizationServiceClient client,
            IMapper mapper,
            ILogger<OrganizationService> logger)
        {
            _client = client;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Guid> CreateOrganizationAsync(CreateOrganizationDto request)
        {
            var protoReq = ExceptionUtils.ExecuteMapping(() => _mapper.Map<KBMGrpcService.Grpc.CreateOrganizationRequest>(request), _logger);
            try
            {
                var reply = await _client.CreateOrganizationAsync(protoReq);
                return _mapper.Map<ResultId<Guid>>(reply).Id;
            }
            catch (RpcException ex)
            {
                throw new ExternalServiceException("Failed to create organization.", ex);
            }
        }

        public async Task<OrganizationDto> GetOrganizationByIdAsync(Guid id)
        {
            try
            {
                var reply = await _client.GetOrganizationByIdAsync(new KBMGrpcService.Grpc.GetOrganizationByIdRequest { Id = id.ToString() });
                return _mapper.Map<OrganizationDto>(reply.Organization);
            }
            catch (RpcException ex)
            {
                throw new ExternalServiceException("Failed to get organization.", ex);
            }
        }

        public async Task<OrganizationListDto> QueryOrganizationsAsync(OrganizationListParamsDto request)
        {
            var protoReq = ExceptionUtils.ExecuteMapping(() => _mapper.Map<KBMGrpcService.Grpc.QueryOrganizationsRequest>(request), _logger);
            try
            {
                var reply = await _client.QueryOrganizationsAsync(protoReq);
                return _mapper.Map<OrganizationListDto>(reply);
            }
            catch (RpcException ex)
            {
                throw new ExternalServiceException("Organization query failed.", ex);
            }
        }

        public async Task UpdateOrganizationAsync(UpdateOrganizationDto request)
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
