using CateringService.Domain.Entities;

namespace CateringService.Domain.Abstractions;

public interface IDishService : IBaseService<Dish, int>
{
    Task<IEnumerable<Dish>> GetAvailableDishesAsync();
}