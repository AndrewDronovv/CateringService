using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.DataTransferObjects.Responses;

namespace CateringService.Domain.Abstractions;

public interface IDishService
{
    Task<DishViewModel> CreateDishAsync(Ulid supplierId, AddDishRequest request);
    Task<DishViewModel> GetByIdAsync(Ulid dishId);
    Task<bool> ToggleDishStateAsync(Ulid dishId);
}