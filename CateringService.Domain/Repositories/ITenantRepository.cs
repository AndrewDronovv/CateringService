using CateringService.Domain.Entities;

namespace CateringService.Domain.Repositories;

public interface ITenantRepository
{
    Task<IEnumerable<Tenant>> GetAllAsync();
    Task<Tenant?> GetByIdAsync(Ulid tenantId);
    Ulid Add(Tenant input);
    void Delete(Tenant tenant);
}