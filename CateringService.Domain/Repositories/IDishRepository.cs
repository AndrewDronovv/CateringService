using CateringService.Domain.Entities;

namespace CateringService.Domain.Repositories;

public interface IDishRepository : IBaseRepository<Dish>
{
    Task<IEnumerable<Dish>> GetAvailableDishesAsync();
}