using CateringService.Domain.Entities.Approved;
using CateringService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CateringService.Persistence.Repositories;

public class MenuCategoryRepository : GenericRepository<MenuCategory, Ulid>, IMenuCategoryRepository
{
    public MenuCategoryRepository(AppDbContext context) : base(context)
    {
    }
    public async Task<MenuCategory> GetMenuCategoryBySupplierIdAsync(Ulid menuCategoryId, Ulid supplierId)
    {
        return await _context.MenuCategories
            .Where(mc => mc.SupplierId == supplierId && mc.Id == menuCategoryId)
            .FirstOrDefaultAsync();
    }

    public async Task<List<MenuCategory>> GetBySupplierIdAsync(Ulid supplierId)
    {
        return await _context.MenuCategories
            .Where(mc => mc.SupplierId == supplierId)
            .ToListAsync();
    }
    public async Task DeleteAsync(Ulid menuCategoryId, Ulid supplierId)
    {
        var entity = await _context.MenuCategories
            .Where(mc => mc.Id == menuCategoryId && mc.SupplierId == supplierId)
            .FirstOrDefaultAsync();

        _context.MenuCategories.Remove(entity!);
    }

    public async Task<bool> HasDishesAsync(Ulid menuCategoryId)
    {
        return await _context.MenuCategories
            .AnyAsync(mc => mc.Id == menuCategoryId && mc.Dishes.Any());
    }

    public async Task<bool> ChechMenuCategoryExists(Ulid menuCategoryId)
    {
        return await _context.MenuCategories
            .AnyAsync(mc => mc.Id == menuCategoryId);
    }
}