﻿using KBMGrpcService.DTOs.Organization;
using KBMGrpcService.Shared.Helpers;

namespace KBMGrpcService.Services.Interfaces
{
    public interface IOrganizationService
    {
        Task<Guid> CreateAsync(CreateOrganizationDto dto);
        Task<OrganizationDto> GetByIdAsync(Guid id);
        Task<PaginatedList<OrganizationDto>> QueryAsync(int page, int pageSize, string orderBy, bool descending, string? query);
        Task UpdateAsync(UpdateOrganizationDto dto);
        Task DeleteAsync(Guid id);
    }
}
