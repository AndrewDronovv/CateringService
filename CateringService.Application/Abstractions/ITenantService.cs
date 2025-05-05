using CateringService.Domain.Entities;

namespace CateringService.Application.Abstractions;

public interface ITenantService
{
    Task<IEnumerable<Tenant>> GetTenantsAsync();
    Task<Tenant?> GetTenantByIdAsync(Ulid tenantId);
    Task<Tenant?> AddAsync(Tenant tenant);
    Task DeleteAsync(Ulid tenantId);
}