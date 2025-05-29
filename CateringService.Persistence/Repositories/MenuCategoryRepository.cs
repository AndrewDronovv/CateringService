using CateringService.Domain.Entities.Approved;
using CateringService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CateringService.Persistence.Repositories;

public class MenuCategoryRepository : GenericRepository<MenuCategory, Ulid>, IMenuCategoryRepository
{
    public MenuCategoryRepository(AppDbContext context) : base(context)
    {
    }
    public async Task<MenuCategory> GetByIdAndSupplierIdAsync(Ulid supplierId, Ulid menuCategoryId)
    {
        var menuCategory = await _context.MenuCategories
            .Where(mc => mc.SupplierId == supplierId && mc.Id == menuCategoryId)
            .FirstOrDefaultAsync();

        return menuCategory;
    }

    public async Task<List<MenuCategory>> GetBySupplierIdAsync(Ulid supplierId)
    {
        return await _context.MenuCategories
            .Where(mc => mc.SupplierId == supplierId)
            .ToListAsync();
    }
    public async Task DeleteAsync(Ulid categoryId, Ulid supplierId)
    {
        var entity = await _context.MenuCategories
            .Where(mc => mc.Id == categoryId && mc.SupplierId == supplierId)
            .FirstOrDefaultAsync();

        _context.MenuCategories.Remove(entity);
    }

    public async Task<bool> HasDishesAsync(Ulid categoryId)
    {
        return await _context.MenuCategories
            .AnyAsync(mc => mc.Id == categoryId && mc.Dishes.Any());
    }
}