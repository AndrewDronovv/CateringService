using CateringService.Domain.Abstractions;
using CateringService.Domain.Entities;

namespace CateringService.Application.Abstractions;

public interface ISupplierService : IBaseService<Supplier, Ulid>
{
    Task<IEnumerable<Supplier>> GetFilteredSuppliersAsync(int workingHours);
    Task<bool> CheckSupplierExists(Ulid supplierId);
}