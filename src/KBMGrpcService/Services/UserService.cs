using AutoMapper;
using KBMGrpcService.Application.DTOs.User;
using KBMGrpcService.Application.Interfaces;
using KBMGrpcService.Common.Exceptions;
using KBMGrpcService.Common.Extensions;
using KBMGrpcService.Common.Helpers;
using KBMGrpcService.Entities;
using KBMGrpcService.Repository;

namespace KBMGrpcService.Application.Services
{
    public class UserService(IUserRepository repository, IMapper mapper) : IUserService
    {
        private readonly IUserRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<Guid> CreateAsync(CreateUserDto dto)
        {
            return await GrpcCustomError.TryCatchAsync(async () =>
            {
                var user = _mapper.Map<User>(dto);
                return await _repository.CreateAsync(user);
            }, "CreateUser", new { dto.Email, dto.Name });
        }

        public async Task<UserDto> GetByIdAsync(Guid id)
        {
            return await GrpcCustomError.TryCatchAsync(async () =>
            {
                var user = await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException("User not found");
                return _mapper.Map<UserDto>(user);
            }, "GetUserById", new { id });
        }

        public async Task<PaginatedList<UserDto>> QueryAsync(int page, int pageSize, string orderBy, bool descending, string? query)
        {
            return await GrpcCustomError.TryCatchAsync(async () =>
            {
                var users = await _repository.QueryAsync(page, pageSize, orderBy, descending, query);
                return users.Map(_mapper.Map<UserDto>);
            }, "QueryUsers", new { page, pageSize, orderBy, descending, query });
        }

        public async Task UpdateAsync(UpdateUserDto dto)
        {
            await GrpcCustomError.TryCatchAsync(async () =>
            {
                var user = await _repository.GetByIdAsync(dto.Id) ?? throw new KeyNotFoundException("User not found");
                _mapper.Map(dto, user);
                user.UpdatedAt = DateTime.UtcNow;
                await _repository.UpdateAsync(user);
            }, "UpdateUser", new { dto.Id });
        }

        public async Task DeleteAsync(Guid id)
        {
            await GrpcCustomError.TryCatchAsync(async () =>
            {
                var user = await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException("User not found");
                user.DeletedAt = DateTime.UtcNow;
                await _repository.UpdateAsync(user);
            }, "DeleteUser", new { id });
        }

        public async Task AssociateAsync(Guid userId, Guid organizationId)
        {
            await GrpcCustomError.TryCatchAsync(() =>
                _repository.AssociateAsync(userId, organizationId),
                "AssociateUser", new { userId, organizationId });
        }

        public async Task DisassociateAsync(Guid userId, Guid organizationId)
        {
            await GrpcCustomError.TryCatchAsync(() =>
                _repository.DisassociateAsync(userId, organizationId),
                "DisassociateUser", new { userId, organizationId });
        }

        public async Task<PaginatedList<UserDto>> QueryForOrganizationAsync(Guid organizationId, int page, int pageSize, string orderBy, bool descending, string? query)
        {
            return await GrpcCustomError.TryCatchAsync(async () =>
            {
                var users = await _repository.QueryForOrganizationAsync(organizationId, page, pageSize, orderBy, descending, query);
                return users.Map(_mapper.Map<UserDto>);
            }, "QueryUsersForOrg", new { organizationId, page, pageSize });
        }
    }
}
