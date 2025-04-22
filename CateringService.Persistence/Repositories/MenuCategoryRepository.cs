using CateringService.Domain.Entities;
using CateringService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CateringService.Persistence.Repositories;

public class MenuCategoryRepository : BaseRepository<MenuCategory, Ulid>, IMenuCategoryRepository
{
    public MenuCategoryRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<MenuCategory>> GetBySupplierIdAsync(Ulid supplierId)
    {
        return await _context.MenuCategories
            .Where(mc => mc.SupplierId == supplierId)
            .Include(mc => mc.Supplier)
            .Include(mc => mc.Dishes)
            .ToListAsync();
    }
}