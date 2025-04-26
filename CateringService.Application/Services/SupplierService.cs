using CateringService.Application.Abstractions;
using CateringService.Domain.Entities;
using CateringService.Domain.Repositories;

namespace CateringService.Application.Services;

public class SupplierService : BaseService<Supplier, Ulid>, ISupplierService
{
    private readonly ISupplierRepository _supplierRepository;
    public SupplierService(ISupplierRepository supplierRepository, IUnitOfWorkRepository unitOfWork) : base(supplierRepository, unitOfWork)
    {
        _supplierRepository = supplierRepository ?? throw new ArgumentNullException(nameof(supplierRepository));
    }

    public Task<bool> CheckSupplierExists(Ulid supplierId)
    {
        return _supplierRepository.CheckSupplierExists(supplierId);
    }
}