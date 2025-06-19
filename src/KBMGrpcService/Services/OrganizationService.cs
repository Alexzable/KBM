using AutoMapper;
using KBMGrpcService.Application.DTOs.Organization;
using KBMGrpcService.Application.Interfaces;
using KBMGrpcService.Common.Helpers;
using KBMGrpcService.Entities;
using KBMGrpcService.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Linq;

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
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx)
            {
                if (sqlEx.Number is 2601 or 2627)
                {
                    var message = sqlEx.Message.ToLower();

                    if (message.Contains("name"))
                        throw new ArgumentException("An organization with this name already exists.", nameof(dto.Name));

                    throw new ArgumentException("An organization with duplicate data already exists.");
                }

                Log.Error(sqlEx, "SQL error creating organization");
                throw;
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
                    .FirstOrDefaultAsync(o => o.Id == id)
                    ?? throw new KeyNotFoundException("Organization not found");
                return _mapper.Map<OrganizationDto>(org);
            }
            catch (SqlException ex)
            {
                Log.Error(ex, "SQL error fetching organization by ID {OrganizationId}", id);
                throw;
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
                IQueryable<Organization> q = _context.Organizations;

                if (!string.IsNullOrEmpty(query))
                    q = q.Where(o => o.Name.Contains(query) || (o.Address != null && o.Address.Contains(query)));

                orderBy = char.ToUpper(orderBy[0]) + orderBy[1..];

                var orgProperties = typeof(Organization).GetProperties().Select(p => p.Name);
                if (!orgProperties.Contains(orderBy))
                {
                    throw new ArgumentException($"Invalid orderBy field '{orderBy}'", nameof(orderBy));
                }

                q = descending
                    ? q.OrderByDescending(e => EF.Property<object>(e, orderBy))
                    : q.OrderBy(e => EF.Property<object>(e, orderBy));

                return await PaginatedList<OrganizationDto>.CreateAsync(q, page, pageSize, _mapper);
            }
            catch (SqlException ex)
            {
                Log.Error(ex, "SQL error querying organizations");
                throw;
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
                    .FirstOrDefaultAsync(o => o.Id == dto.Id)
                    ?? throw new KeyNotFoundException("Organization not found");

                _mapper.Map(dto, org);
                org.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch (SqlException ex)
            {
                Log.Error(ex, "SQL error updating organization {OrganizationId}", dto.Id);
                throw;
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
                    .FirstOrDefaultAsync(o => o.Id == id)
                    ?? throw new KeyNotFoundException("Organization not found");

                org.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch (SqlException ex)
            {
                Log.Error(ex, "SQL error deleting organization {OrganizationId}", id);
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error deleting organization {OrganizationId}", id);
                throw;
            }
        }
    }
}
