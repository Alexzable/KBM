using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using KBMGrpcService.Application.DTOs.User;
using KBMGrpcService.Application.Interfaces;
using KBMGrpcService.Common.Exceptions;

namespace KBMGrpcService.Grpc.Handlers
{
    public class UsersController(IUserService userService, IMapper mapper) : UserService.UserServiceBase
    {
        private readonly IUserService _userService = userService;
        private readonly IMapper _mapper = mapper;

        public override Task<CreateUserReply> CreateUser(CreateUserRequest request, ServerCallContext context)
        {
            return GrpcCustomError.TryCatchAsync(async () =>
            {
                var appDto = _mapper.Map<CreateUserDto>(request);
                var id = await _userService.CreateAsync(appDto);
                return new CreateUserReply { Id = id.ToString() };
            }, "CreateUser", new { request.Email, request.Name });
        }

        public override Task<GetUserByIdReply> GetUserById(GetUserByIdRequest request, ServerCallContext context)
        {
            return GrpcCustomError.TryCatchAsync(async () =>
            {
                var appDto = await _userService.GetByIdAsync(Guid.Parse(request.Id));
                var grpcDto = _mapper.Map<UserMessage>(appDto);
                return new GetUserByIdReply { User = grpcDto };
            }, "GetUserById", new { request.Id });
        }

        public override Task<QueryUsersReply> QueryUsers(QueryUsersRequest request, ServerCallContext context)
        {
            return GrpcCustomError.TryCatchAsync(async () =>
            {
                var result = await _userService.QueryAsync(request.Page, request.PageSize, request.OrderBy, request.Descending, request.QueryString);

                var reply = new QueryUsersReply
                {
                    Page = result.Page,
                    PageSize = result.PageSize,
                    Total = result.Total
                };

                reply.Items.AddRange(result.Items.Select(_mapper.Map<UserMessage>));
                return reply;
            }, "QueryUsers", new { request.Page, request.PageSize, request.OrderBy });
        }

        public override Task<Empty> UpdateUser(UpdateUserRequest request, ServerCallContext context)
        {
            return GrpcCustomError.TryCatchAsync(async () =>
            {
                var appDto = _mapper.Map<UpdateUserDto>(request);
                await _userService.UpdateAsync(appDto);
            }, "UpdateUser", new { request.Id }).ContinueWith(_ => new Empty());
        }

        public override Task<Empty> DeleteUser(DeleteUserRequest request, ServerCallContext context)
        {
            return GrpcCustomError.TryCatchAsync(async () =>
            {
                await _userService.DeleteAsync(Guid.Parse(request.Id));
            }, "DeleteUser", new { request.Id }).ContinueWith(_ => new Empty());
        }

        public override Task<Empty> AssociateUser(AssociateUserRequest request, ServerCallContext context)
        {
            return GrpcCustomError.TryCatchAsync(() =>
                _userService.AssociateAsync(Guid.Parse(request.UserId), Guid.Parse(request.OrganizationId)),
                "AssociateUser", new { request.UserId, request.OrganizationId }).ContinueWith(_ => new Empty());
        }

        public override Task<Empty> DisassociateUser(AssociateUserRequest request, ServerCallContext context)
        {
            return GrpcCustomError.TryCatchAsync(() =>
                _userService.DisassociateAsync(Guid.Parse(request.UserId), Guid.Parse(request.OrganizationId)),
                "DisassociateUser", new { request.UserId, request.OrganizationId }).ContinueWith(_ => new Empty());
        }

        public override Task<QueryUsersReply> QueryUsersForOrganization(QueryUsersForOrganizationRequest request, ServerCallContext context)
        {
            return GrpcCustomError.TryCatchAsync(async () =>
            {
                var result = await _userService.QueryForOrganizationAsync(
                    Guid.Parse(request.OrganizationId),
                    request.Page,
                    request.PageSize,
                    request.OrderBy,
                    request.Descending,
                    request.QueryString);

                var reply = new QueryUsersReply
                {
                    Page = result.Page,
                    PageSize = result.PageSize,
                    Total = result.Total
                };

                reply.Items.AddRange(result.Items.Select(_mapper.Map<UserMessage>));
                return reply;
            }, "QueryUsersForOrganization", new { request.OrganizationId, request.Page, request.PageSize });
        }
    }
}
