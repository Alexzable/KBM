using AutoMapper;
using KBMHttpService.API.Features.User.Models.Requests;
using KBMHttpService.API.Features.User.Models.Responses;

namespace KBMHttpService.Clients.Grpc.User
{
    public class UserGrpcClient : IUserGrpcClient
    {
        private readonly KBMGrpcService.Grpc.UserService.UserServiceClient _client;
        private readonly IMapper _mapper;

        public UserGrpcClient(KBMGrpcService.Grpc.UserService.UserServiceClient client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }

        public async Task<Guid> CreateUserAsync(CreateUserRequest request)
        {
            var protoReq = _mapper.Map<KBMGrpcService.Grpc.CreateUserRequest>(request);
            var reply = await _client.CreateUserAsync(protoReq);
            return _mapper.Map<CreateUserResponse>(reply).Id;
        }

        public async Task<UserResponse> GetUserByIdAsync(Guid id)
        {
            var reply = await _client.GetUserByIdAsync(new KBMGrpcService.Grpc.GetUserByIdRequest { Id = id.ToString() });
            return _mapper.Map<UserResponse>(reply.User);
        }

        public async Task<UsersResponse> QueryUsersAsync(UsersRequest request)
        {
            var protoReq = _mapper.Map<KBMGrpcService.Grpc.QueryUsersRequest>(request);
            var reply = await _client.QueryUsersAsync(protoReq);
            return _mapper.Map<UsersResponse>(reply);
        }

        public async Task UpdateUserAsync(UpdateUserRequest request)
        {
            var protoReq = _mapper.Map<KBMGrpcService.Grpc.UpdateUserRequest>(request);
            await _client.UpdateUserAsync(protoReq);
        }

        public async Task DeleteUserAsync(Guid id)
        {
            await _client.DeleteUserAsync(new KBMGrpcService.Grpc.DeleteUserRequest { Id = id.ToString() });
        }

        public async Task AssociateUserAsync(AssociateUserRequest request)
        {
            var protoReq = _mapper.Map<KBMGrpcService.Grpc.AssociateUserRequest>(request);
            await _client.AssociateUserAsync(protoReq);
        }

        public async Task DisassociateUserAsync(AssociateUserRequest request)
        {
            var protoReq = _mapper.Map<KBMGrpcService.Grpc.AssociateUserRequest>(request);
            await _client.DisassociateUserAsync(protoReq);
        }

        public async Task<UsersResponse> QueryUsersForOrganizationAsync(UsersForOrganizationRequest request)
        {
            var protoReq = _mapper.Map<KBMGrpcService.Grpc.QueryUsersForOrganizationRequest>(request);
            var reply = await _client.QueryUsersForOrganizationAsync(protoReq);
            return _mapper.Map<UsersResponse>(reply);
        }
    }
}
