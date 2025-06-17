using AutoMapper;
using KBMGrpcService.Application.DTOs.User;
using KBMGrpcService.Application.Interfaces;
using KBMGrpcService.Common.Helpers;
using KBMGrpcService.Domain.Entities;
using KBMGrpcService.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace KBMGrpcService.Application.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UserService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Guid> CreateAsync(CreateUserDto dto)
        {
            try
            {
                await using var tx = await _context.Database.BeginTransactionAsync();
                var user = _mapper.Map<User>(dto);
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                await tx.CommitAsync();
                return user.Id;
            }
            catch (SqlException ex)
            {
                Log.Error(ex, "SQL error creating user");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error creating user");
                throw;
            }
        }

        public async Task<UserDto> GetByIdAsync(Guid id)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == id && u.DeletedAt == null)
                    ?? throw new KeyNotFoundException("User not found");
                return _mapper.Map<UserDto>(user);
            }
            catch (SqlException ex)
            {
                Log.Error(ex, "SQL error fetching user by ID {UserId}", id);
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error fetching user by ID {UserId}", id);
                throw;
            }
        }

        public async Task<PaginatedList<UserDto>> QueryAsync(int page, int pageSize, string orderBy, bool descending, string? query)
        {
            try
            {
                var q = _context.Users.Where(u => u.DeletedAt == null);

                if (!string.IsNullOrEmpty(query))
                    q = q.Where(u => u.Name.Contains(query) || u.Username.Contains(query) || u.Email.Value.Contains(query));

                q = descending
                    ? q.OrderByDescending(e => EF.Property<object>(e, orderBy))
                    : q.OrderBy(e => EF.Property<object>(e, orderBy));

                return await PaginatedList<UserDto>.CreateAsync(q, page, pageSize, _mapper);
            }
            catch (SqlException ex)
            {
                Log.Error(ex, "SQL error querying users");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error querying users");
                throw;
            }
        }

        public async Task UpdateAsync(UpdateUserDto dto)
        {
            try
            {
                await using var tx = await _context.Database.BeginTransactionAsync();
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == dto.Id && u.DeletedAt == null)
                    ?? throw new KeyNotFoundException("User not found");

                _mapper.Map(dto, user);
                user.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch (SqlException ex)
            {
                Log.Error(ex, "SQL error updating user {UserId}", dto.Id);
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error updating user {UserId}", dto.Id);
                throw;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                await using var tx = await _context.Database.BeginTransactionAsync();
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == id && u.DeletedAt == null)
                    ?? throw new KeyNotFoundException("User not found");

                user.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch (SqlException ex)
            {
                Log.Error(ex, "SQL error deleting user {UserId}", id);
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error deleting user {UserId}", id);
                throw;
            }
        }

        public async Task AssociateAsync(Guid userId, Guid organizationId)
        {
            try
            {
                await using var tx = await _context.Database.BeginTransactionAsync();
                var exists = await _context.UserOrganizations
                    .AnyAsync(uo => uo.UserId == userId && uo.OrganizationId == organizationId);
                if (!exists)
                {
                    _context.UserOrganizations.Add(new UserOrganization { UserId = userId, OrganizationId = organizationId });
                    await _context.SaveChangesAsync();
                }

                await tx.CommitAsync();
            }
            catch (SqlException ex)
            {
                Log.Error(ex, "SQL error associating user {UserId} with organization {OrgId}", userId, organizationId);
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error associating user {UserId} with organization {OrgId}", userId, organizationId);
                throw;
            }
        }

        public async Task DisassociateAsync(Guid userId, Guid organizationId)
        {
            try
            {
                await using var tx = await _context.Database.BeginTransactionAsync();
                var uo = await _context.UserOrganizations
                    .FirstOrDefaultAsync(u => u.UserId == userId && u.OrganizationId == organizationId);
                if (uo != null)
                {
                    _context.UserOrganizations.Remove(uo);
                    await _context.SaveChangesAsync();
                }

                await tx.CommitAsync();
            }
            catch (SqlException ex)
            {
                Log.Error(ex, "SQL error disassociating user {UserId} from organization {OrgId}", userId, organizationId);
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error disassociating user {UserId} from organization {OrgId}", userId, organizationId);
                throw;
            }
        }

        public async Task<PaginatedList<UserDto>> QueryForOrganizationAsync(Guid organizationId, int page, int pageSize, string orderBy, bool descending, string? query)
        {
            try
            {
                var q = _context.UserOrganizations
                    .Where(uo => uo.OrganizationId == organizationId)
                    .Select(uo => uo.User)
                    .Where(u => u.DeletedAt == null);

                if (!string.IsNullOrEmpty(query))
                    q = q.Where(u => u.Name.Contains(query));

                q = descending
                    ? q.OrderByDescending(e => EF.Property<object>(e, orderBy))
                    : q.OrderBy(e => EF.Property<object>(e, orderBy));

                return await PaginatedList<UserDto>.CreateAsync(q, page, pageSize, _mapper);
            }
            catch (SqlException ex)
            {
                Log.Error(ex, "SQL error querying users for organization {OrgId}", organizationId);
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error querying users for organization {OrgId}", organizationId);
                throw;
            }
        }
    }
}
