using CateringService.Domain.Entities;
using CateringService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CateringService.Persistence.Repositories;

public class SupplierRepository : BaseRepository<Supplier>, ISupplierRepository
{
    public SupplierRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Supplier>> GetActiveSuppliersWithWorkingHoursAsync(int workingHours)
    {
        return await _context.Suppliers
            .Where(s => s.IsActive && s.WorkingHours >= workingHours)
            .ToListAsync();
    }
}