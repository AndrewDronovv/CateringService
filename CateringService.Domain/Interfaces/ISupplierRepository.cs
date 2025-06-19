using CateringService.Domain.Entities.Approved;

namespace CateringService.Domain.Repositories;

public interface ISupplierRepository : IGenericRepository<Supplier, Ulid>
{
    Task<bool> CheckSupplierExists(Ulid suppllierId);
}