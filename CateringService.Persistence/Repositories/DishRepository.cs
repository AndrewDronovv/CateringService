using CateringService.Domain.Entities;
using CateringService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CateringService.Persistence.Repositories;

public class DishRepository : GenericRepository<Dish, Ulid>, IDishRepository
{
    public DishRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Dish?> GetBySlugAsync(string slug)
    {
        return await _context.Dishes
            .FirstOrDefaultAsync(d => d.Slug == slug);
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

    public async Task<IEnumerable<Dish>> GetDishesBySupplierIdAsync(Ulid supplierId)
    {
        return await _context.Dishes.Where(d => d.SupplierId == supplierId).ToListAsync();
    }
}