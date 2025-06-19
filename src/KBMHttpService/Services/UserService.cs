using AutoMapper;
using Grpc.Core;
using KBMHttpService.Common;
using KBMHttpService.Common.Exceptions;
using KBMHttpService.DTOs.User;
using KBMHttpService.Services.Interfaces;

namespace KBMHttpService.Services
{
    public class UserService : IUserService
    {
        private readonly KBMGrpcService.Grpc.UserService.UserServiceClient _client;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;


        public UserService(KBMGrpcService.Grpc.UserService.UserServiceClient client, IMapper mapper, ILogger<UserService> logger)
        {
            _client = client;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Guid> CreateUserAsync(CreateUserDto request)
        {
            var protoReq = ExceptionUtils.ExecuteMapping(() => _mapper.Map<KBMGrpcService.Grpc.CreateUserRequest>(request), _logger);
            try
            {
                var reply = await _client.CreateUserAsync(protoReq);
                return _mapper.Map<ResultId<Guid>>(reply).Id;
            }
            catch (RpcException ex)
            {
                throw new ExternalServiceException("Failed to create user.", ex);
            }
        }

        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            try
            {
                var reply = await _client.GetUserByIdAsync(new KBMGrpcService.Grpc.GetUserByIdRequest { Id = id.ToString() });
                return _mapper.Map<UserDto>(reply.User);
            }
            catch (RpcException ex)
            {
                throw new ExternalServiceException("Failed to get user.", ex);
            }
        }

        public async Task<UserListDto> QueryUsersAsync(UserListParamsDto request)
        {
            var protoReq = ExceptionUtils.ExecuteMapping(() => _mapper.Map<KBMGrpcService.Grpc.QueryUsersRequest>(request), _logger);
            try
            {
                var reply = await _client.QueryUsersAsync(protoReq);
                return _mapper.Map<UserListDto>(reply);
            }
            catch (RpcException ex)
            {
                throw new ExternalServiceException("User query failed.", ex);
            }
        }

        public async Task UpdateUserAsync(UpdateUserDto request)
        {
            var protoReq = ExceptionUtils.ExecuteMapping(() => _mapper.Map<KBMGrpcService.Grpc.UpdateUserRequest>(request), _logger);
            try
            {
                await _client.UpdateUserAsync(protoReq);
            }
            catch (RpcException ex)
            {
                throw new ExternalServiceException("Update failed.", ex);
            }
        }

        public async Task DeleteUserAsync(Guid id)
        {
            try
            {
                await _client.DeleteUserAsync(new KBMGrpcService.Grpc.DeleteUserRequest { Id = id.ToString() });
            }
            catch (RpcException ex)
            {
                throw new ExternalServiceException("Delete failed.", ex);
            }
        }

        public async Task AssociateUserAsync(AssociateUserDto request)
        {
            var protoReq = ExceptionUtils.ExecuteMapping(() => _mapper.Map<KBMGrpcService.Grpc.AssociateUserRequest>(request), _logger);
            try
            {
                await _client.AssociateUserAsync(protoReq);
            }
            catch (RpcException ex)
            {
                throw new ExternalServiceException("Associate failed.", ex);
            }
        }

        public async Task DisassociateUserAsync(AssociateUserDto request)
        {
            var protoReq = ExceptionUtils.ExecuteMapping(() => _mapper.Map<KBMGrpcService.Grpc.AssociateUserRequest>(request), _logger);
            try
            {
                await _client.DisassociateUserAsync(protoReq);
            }
            catch (RpcException ex)
            {
                throw new ExternalServiceException("Disassociate failed.", ex);
            }
        }

        public async Task<UserListDto> QueryUsersForOrganizationAsync(UsersForOrganizationDto request)
        {
            var protoReq = ExceptionUtils.ExecuteMapping(() => _mapper.Map<KBMGrpcService.Grpc.QueryUsersForOrganizationRequest>(request), _logger);
            try
            {
                var reply = await _client.QueryUsersForOrganizationAsync(protoReq);
                return _mapper.Map<UserListDto>(reply);
            }
            catch (RpcException ex)
            {
                throw new ExternalServiceException("Organization user query failed.", ex);
            }
        }
    }
}
