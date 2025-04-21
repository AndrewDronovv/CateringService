using CateringService.Domain.Entities;

namespace CateringService.Domain.Repositories;

public interface ISupplierRepository : IBaseRepository<Supplier, int>
{
    Task<IEnumerable<Supplier>> GetActiveSuppliersWithWorkingHoursAsync(int workingHours);
}
