using AutoMapper;
using CateringService.Application.Abstractions;
using CateringService.Application.DataTransferObjects.Dish;
using CateringService.Domain.Abstractions;
using CateringService.Domain.Entities;

namespace CateringService.Application.Services;

public class DishAppService : IDishAppService
{
    private readonly IDishService _dishService;
    private readonly IMapper _mapper;

    public DishAppService(IDishService dishService, IMapper mapper)
    {
        _dishService = dishService;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DishDto>> GetDishesAsync()
    {
        var dishes = await _dishService.GetAllAsync();
        if (dishes is null)
            throw new ArgumentNullException(nameof(dishes));

        return _mapper.Map<IEnumerable<DishDto>>(dishes);
    }

    public async Task<DishDto> CreateDishAsync(DishCreateDto dishCreatedDto)
    {
        ArgumentNullException.ThrowIfNull(dishCreatedDto);

        var supplierExists = await _dishService.CheckSupplierExistsAsync(dishCreatedDto.SupplierId);
        if (!supplierExists)
        {
            throw new Exception($"Поставщик с Id {dishCreatedDto.SupplierId} не найден.");
        }

        var menuCategoryExists = await _dishService.CheckMenuCategoryExistsAsync(dishCreatedDto.MenuCategoryId);
        if (!menuCategoryExists)
        {
            throw new Exception($"Категория с меню Id {dishCreatedDto.MenuCategoryId}");
        }

        var dish = _mapper.Map<Dish>(dishCreatedDto);
        await _dishService.AddAsync(dish);

        return _mapper.Map<DishDto>(dish);
    }

    public async Task DeleteDishAsync(Ulid id)
    {
        await _dishService.DeleteAsync(id);
    }
    public async Task<DishDto> GetDishByIdAsync(Ulid id)
    {
        var dish = await _dishService.GetByIdAsync(id);

        return _mapper.Map<DishDto>(dish);
    }

    public async Task UpdateDishAsync(Ulid id, DishUpdateDto entity)
    {
        var dish = await _dishService.GetByIdAsync(id);
        if (dish is null)
            throw new KeyNotFoundException(nameof(dish));

        _mapper.Map(entity, dish);

        await _dishService.UpdateAsync(dish);
    }
}