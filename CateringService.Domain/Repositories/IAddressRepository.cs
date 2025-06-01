using CateringService.Domain.Entities.Approved;

namespace CateringService.Domain.Repositories;

public interface IAddressRepository : IGenericRepository<Address, Ulid>
{
    Task<IEnumerable<Address>> SearchByZipAsync(Ulid? tenantId, string zip);
}