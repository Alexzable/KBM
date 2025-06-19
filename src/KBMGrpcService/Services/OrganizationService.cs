using AutoMapper;
using KBMGrpcService.DTOs.Organization;
using KBMGrpcService.Entities;
using KBMGrpcService.Repository;
using KBMGrpcService.Services.Interfaces;
using KBMGrpcService.Shared.Extensions;
using KBMGrpcService.Shared.Helpers;

namespace KBMGrpcService.Services
{
    public class OrganizationService(IOrganizationRepository repository, IMapper mapper) : IOrganizationService
    {
        private readonly IOrganizationRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<Guid> CreateAsync(CreateOrganizationDto dto)
        {
            var org = _mapper.Map<Organization>(dto);
            return await _repository.CreateAsync(org);
        }

        public async Task<OrganizationDto> GetByIdAsync(Guid id)
        {
            var org = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Organization not found");

            return _mapper.Map<OrganizationDto>(org);
        }

        public async Task<PaginatedList<OrganizationDto>> QueryAsync(int page, int pageSize, string orderBy, bool descending, string? query)
        {
            var pagedEntities = await _repository.QueryAsync(page, pageSize, orderBy, descending, query);
            return pagedEntities.Map(_mapper.Map<OrganizationDto>);
        }

        public async Task UpdateAsync(UpdateOrganizationDto dto)
        {
            var org = await _repository.GetByIdAsync(dto.Id)
                ?? throw new KeyNotFoundException("Organization not found");

            _mapper.Map(dto, org);
            org.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(org);
        }

        public async Task DeleteAsync(Guid id)
        {
            var org = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Organization not found");

            await _repository.DeleteAsync(org);
        }
    }
}
