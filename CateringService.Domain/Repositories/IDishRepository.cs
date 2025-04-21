using CateringService.Domain.Entities;

namespace CateringService.Domain.Repositories;

public interface IDishRepository : IBaseRepository<Dish, Ulid>
{
    Task<IEnumerable<Dish>> GetAvailableDishesAsync();
}