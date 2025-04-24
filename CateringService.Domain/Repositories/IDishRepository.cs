using CateringService.Domain.Entities;

namespace CateringService.Domain.Repositories;

public interface IDishRepository : IBaseRepository<Dish, Ulid>
{
    Task<IEnumerable<Dish>> GetAvailableDishesAsync();
    bool CheckSupplierExists(Ulid supplierId);
    bool CheckMenuCategoryExists(Ulid menuCategoryId);
}