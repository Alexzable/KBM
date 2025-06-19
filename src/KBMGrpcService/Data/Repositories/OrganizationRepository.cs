using KBMGrpcService.Entities;
using KBMGrpcService.Repository;
using KBMGrpcService.Shared.Helpers;
using Microsoft.EntityFrameworkCore;

namespace KBMGrpcService.Data.Repositories
{
    public class OrganizationRepository(AppDbContext context) : IOrganizationRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<Guid> CreateAsync(Organization org)
        {
            _context.Organizations.Add(org);
            await _context.SaveChangesAsync();
            return org.Id;
        }

        public async Task<Organization?> GetByIdAsync(Guid id)
        {
            return await _context.Organizations.FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<PaginatedList<Organization>> QueryAsync(int page, int pageSize, string orderBy, bool descending, string? query)
        {
            IQueryable<Organization> q = _context.Organizations;

            if (!string.IsNullOrEmpty(query))
                q = q.Where(o => o.Name.Contains(query) || (o.Address != null && o.Address.Contains(query)));

            orderBy = char.ToUpper(orderBy[0]) + orderBy[1..];
            var orgProperties = typeof(Organization).GetProperties().Select(p => p.Name);

            if (!orgProperties.Contains(orderBy))
                throw new ArgumentException($"Invalid orderBy field '{orderBy}'", nameof(orderBy));

            q = descending
                ? q.OrderByDescending(e => EF.Property<object>(e, orderBy))
                : q.OrderBy(e => EF.Property<object>(e, orderBy));

            return await PaginatedList<Organization>.CreateAsync(q, page, pageSize);
        }

        public async Task UpdateAsync(Organization org)
        {
            _context.Organizations.Update(org);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Organization org)
        {
            org.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}
