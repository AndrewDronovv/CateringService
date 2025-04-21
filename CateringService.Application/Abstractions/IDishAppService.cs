using CateringService.Application.DataTransferObjects.Dish;

namespace CateringService.Application.Abstractions;

public interface IDishAppService
{
    Task<IEnumerable<DishDto>> GetDishesAsync();
    Task<DishDto> CreateDishAsync(DishCreateDto entity);
    Task DeleteDishAsync(Ulid id);
    Task<DishDto> GetDishByIdAsync(Ulid id);
    Task UpdateDishAsync(Ulid id, DishUpdateDto entity);
}