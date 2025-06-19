using AutoMapper;
using KBMGrpcService.Grpc;
using KBMHttpService.Common;
using KBMHttpService.Common.Exceptions;
using KBMHttpService.DTOs.Organization;
using KBMHttpService.Services.Interfaces;

namespace KBMHttpService.Services
{
    public class OrganizationService(
        KBMGrpcService.Grpc.OrganizationService.OrganizationServiceClient client,
        IMapper mapper,
        ILogger<OrganizationService> logger) : IOrganizationService
    {
        private readonly KBMGrpcService.Grpc.OrganizationService.OrganizationServiceClient _client = client;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<OrganizationService> _logger = logger;

        public async Task<Guid> CreateOrganizationAsync(CreateOrganizationDto request)
        {
            var protoReq = ExceptionUtils.ExecuteMapping(() => _mapper.Map<CreateOrganizationRequest>(request), _logger);

            var reply = await ExceptionUtils.ExecuteGrpcCallAsync(
                () => _client.CreateOrganizationAsync(protoReq).ResponseAsync,
                "CreateOrganization",
                _logger);

            return _mapper.Map<ResultId<Guid>>(reply).Id;
        }

        public async Task<OrganizationDto> GetOrganizationByIdAsync(Guid id)
        {
            var reply = await ExceptionUtils.ExecuteGrpcCallAsync(
              () => _client.GetOrganizationByIdAsync(
                  new GetOrganizationByIdRequest { Id = id.ToString() }
              ).ResponseAsync,
              "GetOrganizationById",
              _logger);


            return _mapper.Map<OrganizationDto>(reply.Organization);
        }

        public async Task<OrganizationListDto> QueryOrganizationsAsync(OrganizationListParamsDto request)
        {
            var protoReq = ExceptionUtils.ExecuteMapping(() => _mapper.Map<QueryOrganizationsRequest>(request), _logger);

            var reply = await ExceptionUtils.ExecuteGrpcCallAsync(
                () => _client.QueryOrganizationsAsync(protoReq).ResponseAsync,
                "QueryOrganizations",
                _logger);

            return _mapper.Map<OrganizationListDto>(reply);
        }

        public async Task UpdateOrganizationAsync(UpdateOrganizationDto request)
        {
            var protoReq = ExceptionUtils.ExecuteMapping(() => _mapper.Map<UpdateOrganizationRequest>(request), _logger);

            await ExceptionUtils.ExecuteGrpcCallAsync(
                () => _client.UpdateOrganizationAsync(protoReq).ResponseAsync,
                "UpdateOrganization",
                _logger);
        }

        public async Task DeleteOrganizationAsync(Guid id)
        {
            await ExceptionUtils.ExecuteGrpcCallAsync(
                 () => _client.DeleteOrganizationAsync(
                     new DeleteOrganizationRequest { Id = id.ToString() }
                 ).ResponseAsync,
                 "DeleteOrganization",
                 _logger);
        }

    }
}
