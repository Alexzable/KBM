using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using KBMGrpcService.Application.DTOs.Organization;
using KBMGrpcService.Application.Interfaces;
using KBMGrpcService.Common.Exceptions;

namespace KBMGrpcService.Grpc.Handlers
{
    public class OrganizationsController(IOrganizationService orgService, IMapper mapper) : OrganizationService.OrganizationServiceBase
    {
        private readonly IOrganizationService _orgService = orgService;
        private readonly IMapper _mapper = mapper;

        public override Task<CreateOrganizationReply> CreateOrganization(CreateOrganizationRequest request, ServerCallContext context)
        {
            return GrpcCustomError.TryCatchAsync(async () =>
            {
                var appDto = _mapper.Map<CreateOrganizationDto>(request);
                var id = await _orgService.CreateAsync(appDto);
                return new CreateOrganizationReply { Id = id.ToString() };
            }, "CreateOrganization", new { request.Name });
        }

        public override Task<GetOrganizationByIdReply> GetOrganizationById(GetOrganizationByIdRequest request, ServerCallContext context)
        {
            return GrpcCustomError.TryCatchAsync(async () =>
            {
                var appDto = await _orgService.GetByIdAsync(Guid.Parse(request.Id));
                var grpcDto = _mapper.Map<OrganizationMessage>(appDto);
                return new GetOrganizationByIdReply { Organization = grpcDto };
            }, "GetOrganizationById", new { request.Id });
        }

        public override Task<QueryOrganizationsReply> QueryOrganizations(QueryOrganizationsRequest request, ServerCallContext context)
        {
            return GrpcCustomError.TryCatchAsync(async () =>
            {
                var result = await _orgService.QueryAsync(request.Page, request.PageSize, request.OrderBy, request.Descending, request.QueryString);
                var reply = new QueryOrganizationsReply
                {
                    Page = result.Page,
                    PageSize = result.PageSize,
                    Total = result.Total
                };

                foreach (var dto in result.Items)
                {
                    reply.Items.Add(_mapper.Map<OrganizationMessage>(dto));
                }

                return reply;
            }, "QueryOrganizations", new { request.Page, request.OrderBy, request.QueryString });
        }

        public override Task<Empty> UpdateOrganization(UpdateOrganizationRequest request, ServerCallContext context)
        {
            return GrpcCustomError.TryCatchAsync(async () =>
            {
                var appDto = _mapper.Map<UpdateOrganizationDto>(request);
                await _orgService.UpdateAsync(appDto);
                return new Empty();
            }, "UpdateOrganization", new { request.Id });
        }

        public override Task<Empty> DeleteOrganization(DeleteOrganizationRequest request, ServerCallContext context)
        {
            return GrpcCustomError.TryCatchAsync(async () =>
            {
                await _orgService.DeleteAsync(Guid.Parse(request.Id));
                return new Empty();
            }, "DeleteOrganization", new { request.Id });
        }
    }
}
