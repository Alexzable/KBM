using AutoMapper;
using Grpc.Core;
using KBMGrpcService.Grpc;
using KBMHttpService.Shared.Exceptions;
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
            var protoReq = ExceptionUtils.ExecuteMapping(() => _mapper.Map<CreateUserRequest>(request), _logger);
            var metadata = _metadataFactory.Create();

            var reply = await ExceptionUtils.ExecuteGrpcCallAsync(
                () => _client.CreateUserAsync(protoReq, new CallOptions(metadata)).ResponseAsync,
                "CreateUser", _logger);

            return _mapper.Map<ResultId<Guid>>(reply).Id;
        }

        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            var metadata = _metadataFactory.Create();

            var reply = await ExceptionUtils.ExecuteGrpcCallAsync(
                () => _client.GetUserByIdAsync(new GetUserByIdRequest { Id = id.ToString() }, new CallOptions(metadata)).ResponseAsync,
                "GetUserById", _logger);

            return _mapper.Map<UserDto>(reply.User);
        }

        public async Task<UserListDto> QueryUsersAsync(UserListParamsDto request)
        {
            var protoReq = ExceptionUtils.ExecuteMapping(() => _mapper.Map<QueryUsersRequest>(request), _logger);
            var metadata = _metadataFactory.Create();

            var reply = await ExceptionUtils.ExecuteGrpcCallAsync(
                () => _client.QueryUsersAsync(protoReq, new CallOptions(metadata)).ResponseAsync,
                "QueryUsers", _logger);

            return _mapper.Map<UserListDto>(reply);
        }

        public async Task UpdateUserAsync(UpdateUserDto request)
        {
            var protoReq = ExceptionUtils.ExecuteMapping(() => _mapper.Map<UpdateUserRequest>(request), _logger);
            var metadata = _metadataFactory.Create();

            await ExceptionUtils.ExecuteGrpcCallAsync(
                () => _client.UpdateUserAsync(protoReq, new CallOptions(metadata)).ResponseAsync,
                "UpdateUser", _logger);
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var metadata = _metadataFactory.Create();

            await ExceptionUtils.ExecuteGrpcCallAsync(
                () => _client.DeleteUserAsync(new DeleteUserRequest { Id = id.ToString() }, new CallOptions(metadata)).ResponseAsync,
                "DeleteUser", _logger);
        }

        public async Task AssociateUserAsync(AssociateUserDto request)
        {
            var protoReq = ExceptionUtils.ExecuteMapping(() => _mapper.Map<AssociateUserRequest>(request), _logger);
            var metadata = _metadataFactory.Create();

            await ExceptionUtils.ExecuteGrpcCallAsync(
                () => _client.AssociateUserAsync(protoReq, new CallOptions(metadata)).ResponseAsync,
                "AssociateUser", _logger);
        }

        public async Task DisassociateUserAsync(AssociateUserDto request)
        {
            var protoReq = ExceptionUtils.ExecuteMapping(() => _mapper.Map<AssociateUserRequest>(request), _logger);
            var metadata = _metadataFactory.Create();

            await ExceptionUtils.ExecuteGrpcCallAsync(
                () => _client.DisassociateUserAsync(protoReq, new CallOptions(metadata)).ResponseAsync,
                "DisassociateUser", _logger);
        }

        public async Task<UserListDto> QueryUsersForOrganizationAsync(UsersForOrganizationDto request)
        {
            var protoReq = ExceptionUtils.ExecuteMapping(() => _mapper.Map<QueryUsersForOrganizationRequest>(request), _logger);
            var metadata = _metadataFactory.Create();

            var reply = await ExceptionUtils.ExecuteGrpcCallAsync(
                () => _client.QueryUsersForOrganizationAsync(protoReq, new CallOptions(metadata)).ResponseAsync,
                "QueryUsersForOrganization", _logger);

            return _mapper.Map<UserListDto>(reply);
        }

    }
}
