using CateringService.Domain.Entities;

namespace CateringService.Domain.Repositories;

public interface ITenantRepository
{
    Task<IEnumerable<Tenant>> GetAllAsync();
}
