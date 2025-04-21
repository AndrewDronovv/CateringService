using CateringService.Domain.Entities;
using CateringService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CateringService.Persistence.Repositories;

public class DishRepository : BaseRepository<Dish, Ulid>, IDishRepository
{
    public DishRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Dish>> GetAvailableDishesAsync()
    {
        return await _context.Dishes
            .Where(d => d.AvailabilityStatus)
            .ToListAsync();
    }
}