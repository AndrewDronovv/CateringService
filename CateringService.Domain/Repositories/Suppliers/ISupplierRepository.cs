using CateringService.Domain.Entities;

namespace CateringService.Domain.Repositories.Suppliers;

public interface ISupplierRepository : IRepository<Supplier>
{
    Task<IEnumerable<Supplier>> GetActiveSuppliersWithWorkingHoursAsync(int workingHoures);
}
