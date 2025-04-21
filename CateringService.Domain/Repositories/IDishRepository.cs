using CateringService.Domain.Entities;

namespace CateringService.Domain.Repositories;

public interface IDishRepository : IBaseRepository<Dish, int>
{
    Task<IEnumerable<Dish>> GetAvailableDishesAsync();
}