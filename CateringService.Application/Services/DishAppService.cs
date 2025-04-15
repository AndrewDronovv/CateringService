using AutoMapper;
using CateringService.Application.Abstractions;
using CateringService.Application.DataTransferObjects;
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

    public async Task CreateDishAsync(DishCreateDto entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        var dish = _mapper.Map<Dish>(entity);
        await _dishService.AddAsync(dish);
    }

    public async Task DeleteDishAsync(int id)
    {
        await _dishService.DeleteAsync(id);
    }
    public async Task<DishDto> GetDishByIdAsync(int id)
    {
        var dish = await _dishService.GetByIdAsync(id);

        return _mapper.Map<DishDto>(dish);
    }

    public async Task UpdateDishAsync(int id, DishUpdateDto entity)
    {
        var dish = await _dishService.GetByIdAsync(id);
        if (dish is null)
            throw new KeyNotFoundException(nameof(dish));

        _mapper.Map(entity, dish);

        await _dishService.UpdateAsync(dish);
    }
}