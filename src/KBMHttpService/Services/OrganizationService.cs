using AutoMapper;
using Grpc.Core;
using KBMGrpcService.Grpc;
using KBMHttpService.Shared.Exceptions;
using KBMHttpService.Shared.Helpers;
using KBMHttpService.DTOs.Organization;
using KBMHttpService.Services.Interfaces;

namespace KBMHttpService.Services
{
    public class OrganizationService(
        KBMGrpcService.Grpc.OrganizationService.OrganizationServiceClient client,
        IMapper mapper,
        ILogger<OrganizationService> logger,
        GrpcMetadataFactory metadataFactory) : IOrganizationService
    {
        private readonly KBMGrpcService.Grpc.OrganizationService.OrganizationServiceClient _client = client;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<OrganizationService> _logger = logger;
        private readonly GrpcMetadataFactory _metadataFactory = metadataFactory;

        public async Task<Guid> CreateOrganizationAsync(CreateOrganizationDto request)
        {
            var protoReq = ExceptionUtils.ExecuteMapping(() => _mapper.Map<CreateOrganizationRequest>(request), _logger);
            var metadata = _metadataFactory.Create();

            var reply = await ExceptionUtils.ExecuteGrpcCallAsync(
                () => _client.CreateOrganizationAsync(protoReq, new CallOptions(metadata)).ResponseAsync,
                "CreateOrganization", _logger);

            return _mapper.Map<ResultId<Guid>>(reply).Id;
        }

        public async Task<OrganizationDto> GetOrganizationByIdAsync(Guid id)
        {
            var metadata = _metadataFactory.Create();

            var reply = await ExceptionUtils.ExecuteGrpcCallAsync(
                () => _client.GetOrganizationByIdAsync(new GetOrganizationByIdRequest { Id = id.ToString() }, new CallOptions(metadata)).ResponseAsync,
                "GetOrganizationById", _logger);

            return _mapper.Map<OrganizationDto>(reply.Organization);
        }

        public async Task<OrganizationListDto> QueryOrganizationsAsync(OrganizationListParamsDto request)
        {
            var protoReq = ExceptionUtils.ExecuteMapping(() => _mapper.Map<QueryOrganizationsRequest>(request), _logger);
            var metadata = _metadataFactory.Create();

            var reply = await ExceptionUtils.ExecuteGrpcCallAsync(
                () => _client.QueryOrganizationsAsync(protoReq, new CallOptions(metadata)).ResponseAsync,
                "QueryOrganizations", _logger);

            return _mapper.Map<OrganizationListDto>(reply);
        }

        public async Task UpdateOrganizationAsync(UpdateOrganizationDto request)
        {
            var protoReq = ExceptionUtils.ExecuteMapping(() => _mapper.Map<UpdateOrganizationRequest>(request), _logger);
            var metadata = _metadataFactory.Create();

            await ExceptionUtils.ExecuteGrpcCallAsync(
                () => _client.UpdateOrganizationAsync(protoReq, new CallOptions(metadata)).ResponseAsync,
                "UpdateOrganization", _logger);
        }

        public async Task DeleteOrganizationAsync(Guid id)
        {
            var metadata = _metadataFactory.Create();

            await ExceptionUtils.ExecuteGrpcCallAsync(
                () => _client.DeleteOrganizationAsync(new DeleteOrganizationRequest { Id = id.ToString() }, new CallOptions(metadata)).ResponseAsync,
                "DeleteOrganization", _logger);
        }

    }
}
