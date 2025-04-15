using CateringService.Domain.Entities;

namespace CateringService.Domain.Abstractions;

public interface IDishService : IBaseService<Dish>
{
    Task<IEnumerable<Dish>> GetAvailableDishesAsync();
}