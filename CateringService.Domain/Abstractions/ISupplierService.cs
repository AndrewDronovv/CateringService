using CateringService.Domain.Entities;

namespace CateringService.Application.Abstractions;

public interface ISupplierService
{
    Task<IEnumerable<Supplier>> GetFilteredSuppliersAsync(int workingHours);
}
