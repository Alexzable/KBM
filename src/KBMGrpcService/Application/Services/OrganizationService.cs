using AutoMapper;
using KBMGrpcService.Application.DTOs.Organization;
using KBMGrpcService.Application.Interfaces;
using KBMGrpcService.Common.Helpers;
using KBMGrpcService.Domain.Entities;
using KBMGrpcService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace KBMGrpcService.Application.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public OrganizationService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<Guid> CreateAsync(CreateOrganizationDto dto)
        {
            try
            {
                await using var tx = await _context.Database.BeginTransactionAsync();
                var org = _mapper.Map<Organization>(dto);
                _context.Organizations.Add(org);
                await _context.SaveChangesAsync();
                await tx.CommitAsync();
                return org.Id;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error creating organization");
                throw;
            }
        }

        public async Task<OrganizationDto> GetByIdAsync(Guid id)
        {
            try
            {
                var org = await _context.Organizations
                    .FirstOrDefaultAsync(o => o.Id == id && o.DeletedAt == null)            // -> remove later o.Deleted - check global filter
                    ?? throw new KeyNotFoundException("Organization not found");
                return _mapper.Map<OrganizationDto>(org);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error fetching organization by ID {OrganizationId}", id);
                throw;
            }
        }

        public async Task<PaginatedList<OrganizationDto>> QueryAsync(int page, int pageSize, string orderBy, bool descending, string? query)
        {
            try
            {
                var q = _context.Organizations
                    .Where(o => o.DeletedAt == null);            // -> remove later o.Deleted - check global filter

                if (!string.IsNullOrEmpty(query))
                    q = q.Where(o => o.Name.Contains(query) || (o.Address != null && o.Address.Contains(query)));

                q = descending
                    ? q.OrderByDescending(e => EF.Property<object>(e, orderBy))
                    : q.OrderBy(e => EF.Property<object>(e, orderBy));

                return await PaginatedList<OrganizationDto>.CreateAsync(q, page, pageSize, _mapper);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error querying organizations");
                throw;
            }
        }

        public async Task UpdateAsync(UpdateOrganizationDto dto)
        {
            try
            {
                await using var tx = await _context.Database.BeginTransactionAsync();
                var org = await _context.Organizations
                    .FirstOrDefaultAsync(o => o.Id == dto.Id && o.DeletedAt == null)     // -> remove later o.Deleted - check global filter
                    ?? throw new KeyNotFoundException("Organization not found");

                _mapper.Map(dto, org);
                org.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error updating organization {OrganizationId}", dto.Id);
                throw;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                await using var tx = await _context.Database.BeginTransactionAsync();
                var org = await _context.Organizations
                    .FirstOrDefaultAsync(o => o.Id == id && o.DeletedAt == null)         // -> remove later o.Deleted - check global filter
                    ?? throw new KeyNotFoundException("Organization not found");

                org.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error deleting organization {OrganizationId}", id);
                throw;
            }
        }
    }
}
