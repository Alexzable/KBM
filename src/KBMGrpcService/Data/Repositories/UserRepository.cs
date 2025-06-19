using KBMGrpcService.Entities;
using KBMGrpcService.Repository;
using KBMGrpcService.Shared.Helpers;
using Microsoft.EntityFrameworkCore;

namespace KBMGrpcService.Data.Repositories
{
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<Guid> CreateAsync(User user)
        {
            await using var tx = await _context.Database.BeginTransactionAsync();
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            await tx.CommitAsync();
            return user.Id;
        }


        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(o => o.Id == id);
        }


        public async Task<PaginatedList<User>> QueryAsync(int page, int pageSize, string orderBy, bool descending, string? query)
        {
            IQueryable<User> q = _context.Users;

            if (!string.IsNullOrEmpty(query))
                q = q.Where(u => u.Name.Contains(query) || u.Username.Contains(query) || u.Email.Value.Contains(query));

            orderBy = char.ToUpper(orderBy[0]) + orderBy[1..];
            var props = typeof(User).GetProperties().Select(p => p.Name);
            if (!props.Contains(orderBy))
                throw new ArgumentException($"Invalid orderBy field '{orderBy}'", nameof(orderBy));

            q = descending
                ? q.OrderByDescending(e => EF.Property<object>(e, orderBy))
                : q.OrderBy(e => EF.Property<object>(e, orderBy));

            return await PaginatedList<User>.CreateAsync(q, page, pageSize);
        }

        public async Task UpdateAsync(User user)
        {
            await using var tx = await _context.Database.BeginTransactionAsync();
            await _context.SaveChangesAsync();
            await tx.CommitAsync();
        }

        public async Task DeleteAsync(User user)
        {
            await using var tx = await _context.Database.BeginTransactionAsync();
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            await tx.CommitAsync();
        }

        public async Task AssociateAsync(Guid userId, Guid organizationId)
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

        public async Task DisassociateAsync(Guid userId, Guid organizationId)
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

        public async Task<PaginatedList<User>> QueryForOrganizationAsync(Guid organizationId, int page, int pageSize, string orderBy, bool descending, string? query)
        {
            var q = _context.UserOrganizations
                .Where(uo => uo.OrganizationId == organizationId)
                .Select(uo => uo.User);

            if (!string.IsNullOrEmpty(query))
                q = q.Where(u => u.Name.Contains(query));


            orderBy = char.ToUpper(orderBy[0]) + orderBy[1..];

            var props = typeof(User).GetProperties().Select(p => p.Name);
            if (!props.Contains(orderBy))
                throw new ArgumentException($"Invalid orderBy field '{orderBy}'", nameof(orderBy));
        

            q = descending
                ? q.OrderByDescending(e => EF.Property<object>(e, orderBy))
                : q.OrderBy(e => EF.Property<object>(e, orderBy));

            return await PaginatedList<User>.CreateAsync(q, page, pageSize);
        }
    }

}
