using CateringService.Domain.Entities.Approved;
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
        return await _context.Users
            .AnyAsync(s => s.Id == suppllierId);
    }
}