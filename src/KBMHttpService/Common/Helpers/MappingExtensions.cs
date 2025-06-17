using AutoMapper;
using KBMHttpService.API.Features.Organization.Models.Requests;
using KBMHttpService.API.Features.Organization.Models.Responses;
using KBMHttpService.API.Features.User.Models.Requests;
using KBMHttpService.API.Features.User.Models.Responses;

namespace KBMHttpService.Common.Helpers
{
    public class MappingExtensions : Profile
    {
        public MappingExtensions()
        {

            #region User

            CreateMap<CreateUserRequest, KBMGrpcService.Grpc.CreateUserRequest>();
            CreateMap<UsersRequest, KBMGrpcService.Grpc.QueryUsersRequest>()
                .ForMember(dst => dst.Query, opt => opt.MapFrom(src => src.QueryString));
            CreateMap<UpdateUserRequest, KBMGrpcService.Grpc.UpdateUserRequest>();
            CreateMap<AssociateUserRequest, KBMGrpcService.Grpc.AssociateUserRequest>();
            CreateMap<UsersForOrganizationRequest, KBMGrpcService.Grpc.QueryUsersForOrganizationRequest>()
                .ForMember(dst => dst.Query, opt => opt.MapFrom(src => src.QueryString));

            CreateMap<KBMGrpcService.Grpc.CreateUserReply, CreateUserResponse>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)));
            CreateMap<KBMGrpcService.Grpc.UserMessage, UserResponse>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)))
                .ForMember(dst => dst.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToDateTime()))
                .ForMember(dst => dst.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt.ToDateTime()));
            CreateMap<KBMGrpcService.Grpc.QueryUsersReply, UsersResponse>();

            #endregion

            #region Organization

            CreateMap<CreateOrganizationRequest, KBMGrpcService.Grpc.CreateOrganizationRequest>();
            CreateMap<OrganizationsRequest, KBMGrpcService.Grpc.QueryOrganizationsRequest>()
                .ForMember(d => d.Query, o => o.MapFrom(s => s.QueryString));
            CreateMap<UpdateOrganizationRequest, KBMGrpcService.Grpc.UpdateOrganizationRequest>();

            CreateMap<KBMGrpcService.Grpc.CreateOrganizationReply, CreateOrganizationResponse>()
                .ForMember(d => d.Id, o => o.MapFrom(s => Guid.Parse(s.Id)));
            CreateMap<KBMGrpcService.Grpc.OrganizationMessage, OrganizationResponse>()
                .ForMember(d => d.Id, o => o.MapFrom(s => Guid.Parse(s.Id)))
                .ForMember(d => d.CreatedAt, o => o.MapFrom(s => s.CreatedAt.ToDateTime()))
                .ForMember(d => d.UpdatedAt, o => o.MapFrom(s => s.UpdatedAt.ToDateTime()));
            CreateMap<KBMGrpcService.Grpc.QueryOrganizationsReply, OrganizationsResponse>();

            #endregion
        }
    }
}
