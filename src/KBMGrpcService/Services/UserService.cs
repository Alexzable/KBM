using AutoMapper;
using KBMGrpcService.DTOs.User;
using KBMGrpcService.Entities;
using KBMGrpcService.Repository;
using KBMGrpcService.Services.Interfaces;
using KBMGrpcService.Shared.Extensions;
using KBMGrpcService.Shared.Helpers;

namespace KBMGrpcService.Services
{
    public class UserService(IUserRepository repository, IMapper mapper) : IUserService
    {
        private readonly IUserRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<Guid> CreateAsync(CreateUserDto dto)
        {
            var user = _mapper.Map<User>(dto);
            return await _repository.CreateAsync(user);
        }

        public async Task<UserDto> GetByIdAsync(Guid id)
        {
            var user = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("User not found");

            return _mapper.Map<UserDto>(user);
        }

        public async Task<PaginatedList<UserDto>> QueryAsync(int page, int pageSize, string orderBy, bool descending, string? query)
        {
            var users = await _repository.QueryAsync(page, pageSize, orderBy, descending, query);
            return users.Map(_mapper.Map<UserDto>);
        }

        public async Task UpdateAsync(UpdateUserDto dto)
        {
            var user = await _repository.GetByIdAsync(dto.Id)
                ?? throw new KeyNotFoundException("User not found");

            _mapper.Map(dto, user);
            user.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(user);
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("User not found");

            user.DeletedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(user);
        }

        public async Task AssociateAsync(Guid userId, Guid organizationId)
        {
            await _repository.AssociateAsync(userId, organizationId);
        }

        public async Task DisassociateAsync(Guid userId, Guid organizationId)
        {
            await _repository.DisassociateAsync(userId, organizationId);
        }

        public async Task<PaginatedList<UserDto>> QueryForOrganizationAsync(Guid organizationId, int page, int pageSize, string orderBy, bool descending, string? query)
        {
            var users = await _repository.QueryForOrganizationAsync(organizationId, page, pageSize, orderBy, descending, query);
            return users.Map(_mapper.Map<UserDto>);
        }
    }
}
