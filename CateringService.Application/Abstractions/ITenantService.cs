using CateringService.Domain.Entities;

namespace CateringService.Application.Abstractions;

public interface ITenantService
{
    Task<IEnumerable<Tenant>> GetTenantsAsync();
}