using CateringService.Domain.Entities;
using CateringService.Domain.Repositories.Suppliers;
using Microsoft.EntityFrameworkCore;

namespace CateringService.Persistence.Repositories;

public class SupplierRepository : Repository<Supplier>, ISupplierRepository
{
    private readonly AppDbContext _context;
    public SupplierRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Supplier>> GetActiveSuppliersWithWorkingHoursAsync(int workingHours)
    {
        return await _context.Suppliers
            .Where(s => s.IsActive && s.WorkingHours >= workingHours)
            .ToListAsync();
    }
}