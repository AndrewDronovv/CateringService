using CateringService.Domain.Entities.Approved;
using CateringService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CateringService.Persistence.Repositories;

public class DishRepository : GenericRepository<Dish, Ulid>, IDishRepository
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

    public bool ToggleState(Dish dish)
    {
        if (_context.Entry(dish).State == EntityState.Detached)
        {
            _context.Dishes.Attach(dish);
        }

        _context.Entry(dish).Property(p => p.IsAvailable).IsModified = true;
        return dish.IsAvailable;
    }
}