using CateringService.Domain.Entities;

namespace CateringService.Domain.Repositories;

public interface IAddressRepository : IGenericRepository<Address, Ulid>
{
    Task<IEnumerable<Address>> SearchByZipAsync(Ulid? tenantId, string zip);
    Task<bool> HasActiveOrdersAsync(Ulid addressId);
    Task DeleteAsync(Ulid addressId);
    Task<IEnumerable<Address>> SearchByTextAsync(string query, CancellationToken cancellationToken);
    Task<bool> CheckAddressExistsAsync(Ulid addressId);
}