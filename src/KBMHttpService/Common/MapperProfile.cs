using AutoMapper;
using KBMHttpService.DTOs.Organization;
using KBMHttpService.DTOs.User;

namespace KBMHttpService.Common
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {

            #region User

            CreateMap<CreateUserDto, KBMGrpcService.Grpc.CreateUserRequest>();
            CreateMap<UserListParamsDto, KBMGrpcService.Grpc.QueryUsersRequest>();
            CreateMap<UpdateUserDto, KBMGrpcService.Grpc.UpdateUserRequest>();
            CreateMap<AssociateUserDto, KBMGrpcService.Grpc.AssociateUserRequest>();
            CreateMap<UsersForOrganizationDto, KBMGrpcService.Grpc.QueryUsersForOrganizationRequest>();

            CreateMap<KBMGrpcService.Grpc.CreateUserReply, ResultId<Guid>>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)));
            CreateMap<KBMGrpcService.Grpc.UserMessage, UserDto>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)))
                .ForMember(dst => dst.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToDateTime()))
                .ForMember(dst => dst.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt.ToDateTime()));
            CreateMap<KBMGrpcService.Grpc.QueryUsersReply, UserListDto>();

            #endregion

            #region Organization

            CreateMap<CreateOrganizationDto, KBMGrpcService.Grpc.CreateOrganizationRequest>();
            CreateMap<OrganizationListParamsDto, KBMGrpcService.Grpc.QueryOrganizationsRequest>();
            CreateMap<UpdateOrganizationDto, KBMGrpcService.Grpc.UpdateOrganizationRequest>();

            CreateMap<KBMGrpcService.Grpc.CreateOrganizationReply, ResultId<Guid>>()
                .ForMember(d => d.Id, o => o.MapFrom(s => Guid.Parse(s.Id)));
            CreateMap<KBMGrpcService.Grpc.OrganizationMessage, OrganizationDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => Guid.Parse(s.Id)))
                .ForMember(d => d.CreatedAt, o => o.MapFrom(s => s.CreatedAt.ToDateTime()))
                .ForMember(d => d.UpdatedAt, o => o.MapFrom(s => s.UpdatedAt.ToDateTime()));
            CreateMap<KBMGrpcService.Grpc.QueryOrganizationsReply, OrganizationListDto>();

            #endregion
        }
    }
}
