using CateringService.Domain.Entities;

namespace CateringService.Domain.Repositories;

public interface IDishRepository : IGenericRepository<Dish, Ulid>
{
    Task<IEnumerable<Dish>> GetAvailableDishesAsync();
    bool CheckSupplierExists(Ulid supplierId);
    bool CheckMenuCategoryExists(Ulid menuCategoryId);
}