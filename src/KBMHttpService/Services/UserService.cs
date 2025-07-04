﻿using AutoMapper;
using Grpc.Core;
using KBMGrpcService.Grpc;
using KBMHttpService.Shared.Helpers;
using KBMHttpService.DTOs.User;
using KBMHttpService.Services.Interfaces;

namespace KBMHttpService.Services
{
    public class UserService(
        KBMGrpcService.Grpc.UserService.UserServiceClient client,
        IMapper mapper,
        ILogger<UserService> logger,
        GrpcMetadataFactory metadataFactory) : IUserService
        {
            private readonly KBMGrpcService.Grpc.UserService.UserServiceClient _client = client;
            private readonly IMapper _mapper = mapper;
            private readonly ILogger<UserService> _logger = logger;
            private readonly GrpcMetadataFactory _metadataFactory = metadataFactory;

        public async Task<Guid> CreateUserAsync(CreateUserDto request)
        {
            var protoReq = _mapper.Map<CreateUserRequest>(request);
            var metadata = _metadataFactory.Create();

            var reply = await _client.CreateUserAsync(protoReq, new CallOptions(metadata)).ResponseAsync;
            return _mapper.Map<ResultId<Guid>>(reply).Id;
        }

        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            var metadata = _metadataFactory.Create();

            var reply = await _client.GetUserByIdAsync(
                new GetUserByIdRequest { Id = id.ToString() },
                new CallOptions(metadata)).ResponseAsync;

            return _mapper.Map<UserDto>(reply.User);
        }

        public async Task<UserListDto> QueryUsersAsync(UserListParamsDto request)
        {
            var protoReq = _mapper.Map<QueryUsersRequest>(request);
            var metadata = _metadataFactory.Create();

            var reply = await _client.QueryUsersAsync(protoReq, new CallOptions(metadata)).ResponseAsync;
            return _mapper.Map<UserListDto>(reply);
        }

        public async Task UpdateUserAsync(UpdateUserDto request)
        {
            var protoReq = _mapper.Map<UpdateUserRequest>(request);
            var metadata = _metadataFactory.Create();

            await _client.UpdateUserAsync(protoReq, new CallOptions(metadata)).ResponseAsync;
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var metadata = _metadataFactory.Create();

            await _client.DeleteUserAsync(
                new DeleteUserRequest { Id = id.ToString() },
                new CallOptions(metadata)).ResponseAsync;
        }

        public async Task AssociateUserAsync(AssociateUserDto request)
        {
            var protoReq = _mapper.Map<AssociateUserRequest>(request);
            var metadata = _metadataFactory.Create();

            await _client.AssociateUserAsync(protoReq, new CallOptions(metadata)).ResponseAsync;
        }

        public async Task DisassociateUserAsync(AssociateUserDto request)
        {
            var protoReq = _mapper.Map<AssociateUserRequest>(request);
            var metadata = _metadataFactory.Create();

            await _client.DisassociateUserAsync(protoReq, new CallOptions(metadata)).ResponseAsync;
        }

        public async Task<UserListDto> QueryUsersForOrganizationAsync(UsersForOrganizationDto request)
        {
            var protoReq = _mapper.Map<QueryUsersForOrganizationRequest>(request);
            var metadata = _metadataFactory.Create();

            var reply = await _client.QueryUsersForOrganizationAsync(protoReq, new CallOptions(metadata)).ResponseAsync;
            return _mapper.Map<UserListDto>(reply);
        }

    }
}
