using AutoMapper;
using KBMGrpcService.Application.DTOs.Organization;
using KBMGrpcService.Application.DTOs.User;
using KBMGrpcService.Common.Extensions;
using KBMGrpcService.Domain.Entities;
using KBMGrpcService.Grpc;

namespace KBMGrpcService.Common.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {

            #region organization

            CreateMap<CreateOrganizationDto, Organization>();
            CreateMap<UpdateOrganizationDto, Organization>();

            CreateMap<CreateOrganizationRequest, CreateOrganizationDto>();
            CreateMap<OrganizationDto, OrganizationMessage>();
            CreateMap<UpdateOrganizationRequest, UpdateOrganizationDto>();

            #endregion

            #region user

            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>();

            CreateMap<CreateUserRequest, CreateUserDto>();
            CreateMap<UserDto, UserMessage>();
            CreateMap<UpdateUserRequest, UpdateUserDto>();

            #endregion


        }
    }
}
