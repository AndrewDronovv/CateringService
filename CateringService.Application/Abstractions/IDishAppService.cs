using CateringService.Application.DataTransferObjects;

namespace CateringService.Application.Abstractions;

public interface IDishAppService
{
    Task<IEnumerable<DishDto>> GetDishesAsync();
    Task CreateDishAsync(DishCreateDto entity);
    Task DeleteDishAsync(int id);
    Task<DishDto> GetDishByIdAsync(int id);
    Task UpdateDishAsync(int id, DishUpdateDto entity);
}
