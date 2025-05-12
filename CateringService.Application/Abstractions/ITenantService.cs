using CateringService.Application.DataTransferObjects.Tenants;
using CateringService.Domain.Entities;

namespace CateringService.Application.Abstractions;

public interface ITenantService
{
    Task<IEnumerable<Tenant>> GetTenantsAsync();
    Task<Tenant?> GetTenantByIdAsync(Ulid tenantId);
    Task<Tenant?> AddTenantAsync(Tenant tenant);
    Task DeleteAsync(Ulid tenantId);
    Task<Tenant> UpdateTenantAsync(Ulid tenantId, Tenant tenant);
    Task<TenantDto> BlockTenantAsync(Ulid tenantId, string blockReason);
}