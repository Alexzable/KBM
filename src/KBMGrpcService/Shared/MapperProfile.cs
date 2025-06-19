using AutoMapper;
using KBMGrpcService.DTOs.Organization;
using KBMGrpcService.DTOs.User;
using KBMGrpcService.Entities;
using KBMGrpcService.Grpc;
using KBMGrpcService.Shared.Extensions;

namespace KBMGrpcService.Shared.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {

            #region organization

            CreateMap<Organization, OrganizationDto>();
            CreateMap<OrganizationDto, OrganizationMessage>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToGrpcTimestamp()))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt.ToNullableGrpcTimestamp()));

            CreateMap<CreateOrganizationDto, Organization>();
            CreateMap<UpdateOrganizationDto, Organization>();

            CreateMap<CreateOrganizationRequest, CreateOrganizationDto>();
            CreateMap<UpdateOrganizationRequest, UpdateOrganizationDto>();

            #endregion

            #region user

            CreateMap<User, UserDto>();
            CreateMap<UserDto, UserMessage>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToGrpcTimestamp()))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt.ToNullableGrpcTimestamp()));

            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>();

            CreateMap<CreateUserRequest, CreateUserDto>();
            CreateMap<UpdateUserRequest, UpdateUserDto>();

            #endregion


        }
    }
}
