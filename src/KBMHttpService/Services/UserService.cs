using AutoMapper;
using KBMGrpcService.Grpc;
using KBMHttpService.Common;
using KBMHttpService.Common.Exceptions;
using KBMHttpService.DTOs.User;
using KBMHttpService.Services.Interfaces;

namespace KBMHttpService.Services
{
    public class UserService(KBMGrpcService.Grpc.UserService.UserServiceClient client, IMapper mapper, ILogger<UserService> logger) : IUserService
    {
        private readonly KBMGrpcService.Grpc.UserService.UserServiceClient _client = client;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UserService> _logger = logger;

        public async Task<Guid> CreateUserAsync(CreateUserDto request)
        {
            var protoReq = ExceptionUtils.ExecuteMapping(() => _mapper.Map<CreateUserRequest>(request), _logger);

            var reply = await ExceptionUtils.ExecuteGrpcCallAsync(
                () => _client.CreateUserAsync(protoReq).ResponseAsync,
                "CreateUser", _logger);

            return _mapper.Map<ResultId<Guid>>(reply).Id;
        }

        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            var reply = await ExceptionUtils.ExecuteGrpcCallAsync(
                () => _client.GetUserByIdAsync(new GetUserByIdRequest { Id = id.ToString() }).ResponseAsync,
                "GetUserById", _logger);

            return _mapper.Map<UserDto>(reply.User);
        }

        public async Task<UserListDto> QueryUsersAsync(UserListParamsDto request)
        {
            var protoReq = ExceptionUtils.ExecuteMapping(() => _mapper.Map<QueryUsersRequest>(request), _logger);

            var reply = await ExceptionUtils.ExecuteGrpcCallAsync(
                () => _client.QueryUsersAsync(protoReq).ResponseAsync,
                "QueryUsers", _logger);

            return _mapper.Map<UserListDto>(reply);
        }

        public async Task UpdateUserAsync(UpdateUserDto request)
        {
            var protoReq = ExceptionUtils.ExecuteMapping(() => _mapper.Map<UpdateUserRequest>(request), _logger);

            await ExceptionUtils.ExecuteGrpcCallAsync(
                () => _client.UpdateUserAsync(protoReq).ResponseAsync,
                "UpdateUser", _logger);
        }

        public async Task DeleteUserAsync(Guid id)
        {
            await ExceptionUtils.ExecuteGrpcCallAsync(
                () => _client.DeleteUserAsync(new DeleteUserRequest { Id = id.ToString() }).ResponseAsync,
                "DeleteUser", _logger);
        }

        public async Task AssociateUserAsync(AssociateUserDto request)
        {
            var protoReq = ExceptionUtils.ExecuteMapping(() => _mapper.Map<AssociateUserRequest>(request), _logger);

            await ExceptionUtils.ExecuteGrpcCallAsync(
                () => _client.AssociateUserAsync(protoReq).ResponseAsync,
                "AssociateUser", _logger);
        }

        public async Task DisassociateUserAsync(AssociateUserDto request)
        {
            var protoReq = ExceptionUtils.ExecuteMapping(() => _mapper.Map<AssociateUserRequest>(request), _logger);

            await ExceptionUtils.ExecuteGrpcCallAsync(
                () => _client.DisassociateUserAsync(protoReq).ResponseAsync,
                "DisassociateUser", _logger);
        }

        public async Task<UserListDto> QueryUsersForOrganizationAsync(UsersForOrganizationDto request)
        {
            var protoReq = ExceptionUtils.ExecuteMapping(() => _mapper.Map<QueryUsersForOrganizationRequest>(request), _logger);

            var reply = await ExceptionUtils.ExecuteGrpcCallAsync(
                () => _client.QueryUsersForOrganizationAsync(protoReq).ResponseAsync,
                "QueryUsersForOrganization", _logger);

            return _mapper.Map<UserListDto>(reply);
        }
    }
}
