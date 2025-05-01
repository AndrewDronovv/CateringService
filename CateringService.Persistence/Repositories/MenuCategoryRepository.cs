using CateringService.Domain.Entities;
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
            .Where(mc => mc.SupplierId == menuCategoryId && mc.Id == supplierId)
            .FirstOrDefaultAsync();

        if (menuCategory == null)
        {
            throw new Exception("Категория меню не найдена");
        }

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

        if (entity == null)
        {
            throw new KeyNotFoundException($"Категория меню с Id = {categoryId} или поставщик с Id = {supplierId} не найдены.");
        }

        _context.MenuCategories.Remove(entity);
    }

    public async Task<bool> HasDishesAsync(Ulid categoryId)
    {
        return await _context.MenuCategories
            .AnyAsync(mc => mc.Id == categoryId && mc.Dishes.Any());
    }
}