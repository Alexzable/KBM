using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using KBMGrpcService.Application.DTOs.Organization;
using KBMGrpcService.Application.Interfaces;
using KBMGrpcService.Common.Exceptions;

namespace KBMGrpcService.Grpc.Handlers
{
    public class OrganizationController : OrganizationService.OrganizationServiceBase
    {
        private readonly IOrganizationService _orgService;
        private readonly IMapper _mapper;

        public OrganizationController(IOrganizationService orgService, IMapper mapper)
        {
            _orgService = orgService;
            _mapper = mapper;
        }


        public override async Task<CreateOrganizationReply> CreateOrganization(CreateOrganizationRequest request, ServerCallContext context)
        {
            try
            {
                var appDto = _mapper.Map<CreateOrganizationDto>(request);
                var id = await _orgService.CreateAsync(appDto);
                return new CreateOrganizationReply { Id = id.ToString() };
            }
            catch (Exception ex)
            {
                throw GrpcErrorCustom.FromException(ex);
            }
        }

        public override async Task<GetOrganizationByIdReply> GetOrganizationById(GetOrganizationByIdRequest request, ServerCallContext context)
        {
            try
            {
                var appDto = await _orgService.GetByIdAsync(Guid.Parse(request.Id));
                var grpcDto = _mapper.Map<OrganizationMessage>(appDto);
                return new GetOrganizationByIdReply { Organization = grpcDto };
            }
            catch (Exception ex)
            {
                throw GrpcErrorCustom.FromException(ex);
            }
        }

        public override async Task<QueryOrganizationsReply> QueryOrganizations(QueryOrganizationsRequest request, ServerCallContext context)
        {
            try
            {
                var result = await _orgService.QueryAsync(request.Page, request.PageSize, request.OrderBy, request.Descending, request.QueryString);

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
            catch (Exception ex)
            {
                throw GrpcErrorCustom.FromException(ex);
            }
        }

        public override async Task<Empty> UpdateOrganization(UpdateOrganizationRequest request, ServerCallContext context)
        {
            try
            {
                var appDto = _mapper.Map<UpdateOrganizationDto>(request);
                await _orgService.UpdateAsync(appDto);
                return new Empty();
            }
            catch (Exception ex)
            {
                throw GrpcErrorCustom.FromException(ex);
            }
        }

        public override async Task<Empty> DeleteOrganization(DeleteOrganizationRequest request, ServerCallContext context)
        {
            try
            {
                await _orgService.DeleteAsync(Guid.Parse(request.Id));
                return new Empty();
            }
            catch (Exception ex)
            {
                throw GrpcErrorCustom.FromException(ex);
            }
        }

    }
}
