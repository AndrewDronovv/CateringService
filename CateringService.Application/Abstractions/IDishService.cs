using CateringService.Domain.Entities;

namespace CateringService.Domain.Abstractions;

public interface IDishService : IBaseService<Dish, Ulid>
{
    bool CheckSupplierExists(Ulid supplierId);
    bool CheckMenuCategoryExists(Ulid menuCategoryId);
    Task<bool> ToggleDishState(Ulid dishId);
}