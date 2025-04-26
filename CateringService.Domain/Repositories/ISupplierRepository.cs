using CateringService.Domain.Entities;

namespace CateringService.Domain.Repositories;

public interface ISupplierRepository : IGenericRepository<Supplier, Ulid>
{
    Task<IEnumerable<Supplier>> GetActiveSuppliersWithWorkingHoursAsync(int workingHours);
    Task<bool> CheckSupplierExists(Ulid suppllierId);
}
