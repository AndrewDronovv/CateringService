using CateringService.Domain.Entities;
using CateringService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CateringService.Persistence.Repositories;

public sealed class SupplierRepository : GenericRepository<Supplier, Ulid>, ISupplierRepository
{
    public SupplierRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<bool> CheckSupplierExists(Ulid suppllierId)
    {
        return await _context.Suppliers
            .AnyAsync(s => s.Id == suppllierId);
    }
}