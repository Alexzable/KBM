using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using KBMGrpcService.Application.DTOs.Organization;
using KBMGrpcService.Application.Interfaces;

namespace KBMGrpcService.Grpc.Handlers
{
    public class OrganizationHandler : OrganizationService.OrganizationServiceBase
    {
        private readonly IOrganizationService _orgService;
        private readonly IMapper _mapper;

        public OrganizationHandler(IOrganizationService orgService, IMapper mapper)
        {
            _orgService = orgService;
            _mapper = mapper;
        }


        public override async Task<CreateOrganizationReply> CreateOrganization(CreateOrganizationRequest request, ServerCallContext context)
        {
            var appDto = _mapper.Map<CreateOrganizationDto>(request);

            var id = await _orgService.CreateAsync(appDto);

            return new CreateOrganizationReply { Id = id.ToString() };
        }

        public override async Task<GetOrganizationByIdReply> GetOrganizationById(GetOrganizationByIdRequest request, ServerCallContext context)
        {
            var appDto = await _orgService.GetByIdAsync(Guid.Parse(request.Id));

            var grpcDto = _mapper.Map<OrganizationMessage>(appDto);

            return new GetOrganizationByIdReply { Organization = grpcDto };
        }

        public override async Task<QueryOrganizationsReply> QueryOrganizations(QueryOrganizationsRequest request, ServerCallContext context)
        {
            var result = await _orgService.QueryAsync(request.Page, request.PageSize, request.OrderBy, request.Descending, request.Query);

            var reply = new QueryOrganizationsReply
            {
                Page = result.Page,
                PageSize = result.PageSize,
                Total = result.Total
            };

            foreach (var appDto in result.Items)
            {
                reply.Items.Add(_mapper.Map<OrganizationMessage>(appDto));
            }

            return reply;
        }

        public override async Task<Empty> UpdateOrganization(UpdateOrganizationRequest request, ServerCallContext context)
        {
            var appDto = _mapper.Map<UpdateOrganizationDto>(request);

            await _orgService.UpdateAsync(appDto);
            return new Empty();
        }

        public override async Task<Empty> DeleteOrganization(DeleteOrganizationRequest request, ServerCallContext context)
        {
            await _orgService.DeleteAsync(Guid.Parse(request.Id));
            return new Empty();
        }
    
    }
}
