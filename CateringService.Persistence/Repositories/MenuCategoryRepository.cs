using CateringService.Domain.Entities;
using CateringService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CateringService.Persistence.Repositories;

public class MenuCategoryRepository : BaseRepository<MenuCategory, Ulid>, IMenuCategoryRepository
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
}