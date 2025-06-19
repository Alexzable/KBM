using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using KBMGrpcService.Application.DTOs.User;
using KBMGrpcService.Application.Interfaces;
using KBMGrpcService.Common.Exceptions;

namespace KBMGrpcService.Grpc.Handlers
{
    public class UserController : UserService.UserServiceBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public override async Task<CreateUserReply> CreateUser(CreateUserRequest request, ServerCallContext context)
        {
            try
            {
                var appDto = _mapper.Map<CreateUserDto>(request);
                var id = await _userService.CreateAsync(appDto);
                return new CreateUserReply { Id = id.ToString() };
            }
            catch (Exception ex)
            {
                throw GrpcErrorCustom.FromException(ex);
            }
        }

        public override async Task<GetUserByIdReply> GetUserById(GetUserByIdRequest request, ServerCallContext context)
        {
            try
            {
                var appDto = await _userService.GetByIdAsync(Guid.Parse(request.Id));
                var grpcDto = _mapper.Map<UserMessage>(appDto);
                return new GetUserByIdReply { User = grpcDto };
            }
            catch (Exception ex)
            {
                throw GrpcErrorCustom.FromException(ex);
            }
        }

        public override async Task<QueryUsersReply> QueryUsers(QueryUsersRequest request, ServerCallContext context)
        {
            try
            {
                var result = await _userService.QueryAsync(request.Page, request.PageSize, request.OrderBy, request.Descending, request.QueryString);

                var reply = new QueryUsersReply
                {
                    Page = result.Page,
                    PageSize = result.PageSize,
                    Total = result.Total
                };

                foreach (var appDto in result.Items)
                {
                    reply.Items.Add(_mapper.Map<UserMessage>(appDto));
                }

                return reply;
            }
            catch (Exception ex)
            {
                throw GrpcErrorCustom.FromException(ex);
            }
        }

        public override async Task<Empty> UpdateUser(UpdateUserRequest request, ServerCallContext context)
        {
            try
            {
                var appDto = _mapper.Map<UpdateUserDto>(request);
                await _userService.UpdateAsync(appDto);
                return new Empty();
            }
            catch (Exception ex)
            {
                throw GrpcErrorCustom.FromException(ex);
            }
        }

        public override async Task<Empty> DeleteUser(DeleteUserRequest request, ServerCallContext context)
        {
            try
            {
                await _userService.DeleteAsync(Guid.Parse(request.Id));
                return new Empty();
            }
            catch (Exception ex)
            {
                throw GrpcErrorCustom.FromException(ex);
            }
        }

        public override async Task<Empty> AssociateUser(AssociateUserRequest request, ServerCallContext context)
        {
            try
            {
                await _userService.AssociateAsync(Guid.Parse(request.UserId), Guid.Parse(request.OrganizationId));
                return new Empty();
            }
            catch (Exception ex)
            {
                throw GrpcErrorCustom.FromException(ex);
            }
        }

        public override async Task<Empty> DisassociateUser(AssociateUserRequest request, ServerCallContext context)
        {
            try
            {
                await _userService.DisassociateAsync(Guid.Parse(request.UserId), Guid.Parse(request.OrganizationId));
                return new Empty();
            }
            catch (Exception ex)
            {
                throw GrpcErrorCustom.FromException(ex);
            }
        }

        public override async Task<QueryUsersReply> QueryUsersForOrganization(QueryUsersForOrganizationRequest request, ServerCallContext context)
        {
            try
            {
                var result = await _userService.QueryForOrganizationAsync(
                    Guid.Parse(request.OrganizationId), request.Page, request.PageSize, request.OrderBy, request.Descending, request.QueryString);

                var reply = new QueryUsersReply
                {
                    Page = result.Page,
                    PageSize = result.PageSize,
                    Total = result.Total
                };

                foreach (var appDto in result.Items)
                {
                    reply.Items.Add(_mapper.Map<UserMessage>(appDto));
                }

                return reply;
            }
            catch (Exception ex)
            {
                throw GrpcErrorCustom.FromException(ex);
            }
        }
    }
}
