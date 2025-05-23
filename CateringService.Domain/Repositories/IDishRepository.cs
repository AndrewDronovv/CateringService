using CateringService.Domain.Entities.Approved;

namespace CateringService.Domain.Repositories;

public interface IDishRepository : IGenericRepository<Dish, Ulid>
{
    bool CheckSupplierExists(Ulid supplierId);
    bool CheckMenuCategoryExists(Ulid menuCategoryId);
    bool ToggleState(Dish dish);
}