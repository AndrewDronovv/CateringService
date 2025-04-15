using CateringService.Application.Abstractions;
using CateringService.Domain.Entities;
using CateringService.Domain.Repositories.Suppliers;

namespace CateringService.Application.Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _supplierRepository;

    public SupplierService(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public Task<IEnumerable<Supplier>> GetFilteredSuppliersAsync(int workingHours)
    {
        return _supplierRepository.GetActiveSuppliersWithWorkingHoursAsync(workingHours);
    }
}
