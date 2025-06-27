using CateringService.Application.Abstractions;
using CateringService.Domain.Repositories;

namespace CateringService.Application.Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _supplierRepository;
    public SupplierService(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository ?? throw new ArgumentNullException(nameof(supplierRepository));
    }
}