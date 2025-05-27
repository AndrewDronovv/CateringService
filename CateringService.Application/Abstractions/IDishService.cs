using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.DataTransferObjects.Responses;

namespace CateringService.Domain.Abstractions;

public interface IDishService
{
    Task<DishViewModel> CreateDishAsync(AddDishRequest request, Ulid supplierId);
    bool CheckSupplierExists(Ulid supplierId);
    bool CheckMenuCategoryExists(Ulid menuCategoryId);
    Task<bool> ToggleDishStateAsync(Ulid dishId);
}