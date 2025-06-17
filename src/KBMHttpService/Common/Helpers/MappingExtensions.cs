using AutoMapper;
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
        }
    }
}
