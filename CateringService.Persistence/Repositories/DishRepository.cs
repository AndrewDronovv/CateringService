using CateringService.Domain.Entities;
using CateringService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CateringService.Persistence.Repositories;

public class DishRepository : BaseRepository<Dish, Ulid>, IDishRepository
{
    public DishRepository(AppDbContext context) : base(context)
    {
    }

    public bool CheckMenuCategoryExists(Ulid menuCategoryId)
    {
        return _context.MenuCategories
            .Any(mc => mc.Id == menuCategoryId);
    }

    public bool CheckSupplierExists(Ulid supplierId)
    {
        return _context.Suppliers
            .Any(s => s.Id == supplierId);
    }

    public async Task<IEnumerable<Dish>> GetAvailableDishesAsync()
    {
        return await _context.Dishes
            .Where(d => d.IsAvailable)
            .ToListAsync();
    }
}