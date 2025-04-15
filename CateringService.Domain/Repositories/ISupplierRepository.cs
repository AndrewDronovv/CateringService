using CateringService.Domain.Entities;

namespace CateringService.Domain.Repositories;

public interface ISupplierRepository : IBaseRepository<Supplier>
{
    Task<IEnumerable<Supplier>> GetActiveSuppliersWithWorkingHoursAsync(int workingHoures);
}
