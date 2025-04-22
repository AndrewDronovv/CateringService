using CateringService.Application.Abstractions;
using CateringService.Domain.Entities;
using CateringService.Domain.Repositories;

namespace CateringService.Application.Services;

public class SupplierService : BaseService<Supplier, Ulid>, ISupplierService
{
    private readonly ISupplierRepository _supplierRepository;
    public SupplierService(ISupplierRepository supplierRepository, IUnitOfWorkRepository unitOfWork) : base(supplierRepository, unitOfWork)
    {
        _supplierRepository = supplierRepository;
    }

    public Task<IEnumerable<Supplier>> GetFilteredSuppliersAsync(int workingHours)
    {
        return _supplierRepository.GetActiveSuppliersWithWorkingHoursAsync(workingHours);
    }
}
