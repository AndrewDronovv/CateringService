using CateringService.Domain.Entities;
using CateringService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CateringService.Persistence.Repositories;

public class DishRepository : BaseRepository<Dish, Ulid>, IDishRepository
{
    public DishRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<bool> CheckMenuCategoryExistsAsync(Ulid menuCategoryId)
    {
        return await _context.MenuCategories.AnyAsync(mc => mc.Id == menuCategoryId);
    }

    public async Task<bool> CheckSupplierExistsAsync(Ulid supplierId)
    {
        return await _context.Suppliers.AnyAsync(s => s.Id == supplierId);
    }

    public async Task<IEnumerable<Dish>> GetAvailableDishesAsync()
    {
        return await _context.Dishes
            .Where(d => d.IsAvailable)
            .ToListAsync();
    }
}