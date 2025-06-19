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
            var protoReq = _mapper.Map<CreateOrganizationRequest>(request);
            var metadata = _metadataFactory.Create();

            var reply = await _client.CreateOrganizationAsync(protoReq, new CallOptions(metadata)).ResponseAsync;
            return _mapper.Map<ResultId<Guid>>(reply).Id;
        }

        public async Task<OrganizationDto> GetOrganizationByIdAsync(Guid id)
        {
            var metadata = _metadataFactory.Create();

            var reply = await _client.GetOrganizationByIdAsync(
                new GetOrganizationByIdRequest { Id = id.ToString() },
                new CallOptions(metadata)).ResponseAsync;

            return _mapper.Map<OrganizationDto>(reply.Organization);
        }

        public async Task<OrganizationListDto> QueryOrganizationsAsync(OrganizationListParamsDto request)
        {
            var protoReq = _mapper.Map<QueryOrganizationsRequest>(request);
            var metadata = _metadataFactory.Create();

            var reply = await _client.QueryOrganizationsAsync(protoReq, new CallOptions(metadata)).ResponseAsync;
            return _mapper.Map<OrganizationListDto>(reply);
        }

        public async Task UpdateOrganizationAsync(UpdateOrganizationDto request)
        {
            var protoReq = _mapper.Map<UpdateOrganizationRequest>(request);
            var metadata = _metadataFactory.Create();

            await _client.UpdateOrganizationAsync(protoReq, new CallOptions(metadata)).ResponseAsync;
        }

        public async Task DeleteOrganizationAsync(Guid id)
        {
            var metadata = _metadataFactory.Create();

            await _client.DeleteOrganizationAsync(
                new DeleteOrganizationRequest { Id = id.ToString() },
                new CallOptions(metadata)).ResponseAsync;
        }
    }
}
