using CateringService.Domain.Abstractions;
using CateringService.Domain.Entities.Approved;

namespace CateringService.Application.Abstractions;

public interface ISupplierService : IBaseService<Supplier, Ulid>
{
    Task<bool> CheckSupplierExists(Ulid supplierId);
}