using CateringService.Domain.Entities;

namespace CateringService.Domain.Abstractions;

public interface IDishService : IBaseService<Dish, Ulid>
{
    Task<IEnumerable<Dish>> GetAvailableDishesAsync();
}