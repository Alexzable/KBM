using AutoMapper;
using KBMGrpcService.Application.DTOs.Organization;
using KBMGrpcService.Application.Interfaces;
using KBMGrpcService.Common.Exceptions;
using KBMGrpcService.Common.Extensions;
using KBMGrpcService.Common.Helpers;
using KBMGrpcService.Entities;
using KBMGrpcService.Repository;

namespace KBMGrpcService.Application.Services
{
    public class OrganizationService(IOrganizationRepository repository, IMapper mapper) : IOrganizationService
    {
        private readonly IOrganizationRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<Guid> CreateAsync(CreateOrganizationDto dto)
        {
            return await GrpcCustomError.TryCatchAsync(async () =>
            {
                var org = _mapper.Map<Organization>(dto);
                return await _repository.CreateAsync(org);
            }, "CreateOrganization", new { dto.Name });
        }

        public async Task<OrganizationDto> GetByIdAsync(Guid id)
        {
            return await GrpcCustomError.TryCatchAsync(async () =>
            {
                var org = await _repository.GetByIdAsync(id)
                    ?? throw new KeyNotFoundException("Organization not found");

                return _mapper.Map<OrganizationDto>(org);
            }, "GetOrganizationById", new { id });
        }

        public async Task<PaginatedList<OrganizationDto>> QueryAsync(int page, int pageSize, string orderBy, bool descending, string? query)
        {
            return await GrpcCustomError.TryCatchAsync(async () =>
            {
                var pagedEntities = await _repository.QueryAsync(page, pageSize, orderBy, descending, query);
                return pagedEntities.Map(_mapper.Map<OrganizationDto>);
            }, "QueryOrganizations", new { page, pageSize, orderBy, descending, query });
        }

        public async Task UpdateAsync(UpdateOrganizationDto dto)
        {
            await GrpcCustomError.TryCatchAsync(async () =>
            {
                var org = await _repository.GetByIdAsync(dto.Id)
                    ?? throw new KeyNotFoundException("Organization not found");

                _mapper.Map(dto, org);
                org.UpdatedAt = DateTime.UtcNow;

                await _repository.UpdateAsync(org);
            }, "UpdateOrganization", new { dto.Id });
        }

        public async Task DeleteAsync(Guid id)
        {
            await GrpcCustomError.TryCatchAsync(async () =>
            {
                var org = await _repository.GetByIdAsync(id)
                    ?? throw new KeyNotFoundException("Organization not found");

                await _repository.DeleteAsync(org);
            }, "DeleteOrganization", new { id });
        }
    }
}
