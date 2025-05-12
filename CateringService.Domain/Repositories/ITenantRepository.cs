using CateringService.Domain.Common;
using CateringService.Domain.Entities;

namespace CateringService.Domain.Repositories;

public interface ITenantRepository
{
    Task<IEnumerable<Tenant>> GetAllAsync();
    Task<Tenant?> GetByIdAsync(Ulid tenantId);
    Ulid Add(Tenant tenant);
    void Delete(Tenant tenant);
    Task<Tenant> UpdateAsync(Tenant tenant, bool isNotTracked = false);
    Task BlockAsync(Ulid tenantId, string blockReason);
}