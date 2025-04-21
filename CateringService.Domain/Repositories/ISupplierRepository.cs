using CateringService.Domain.Entities;

namespace CateringService.Domain.Repositories;

public interface ISupplierRepository : IBaseRepository<Supplier, Ulid>
{
    Task<IEnumerable<Supplier>> GetActiveSuppliersWithWorkingHoursAsync(int workingHours);
}
