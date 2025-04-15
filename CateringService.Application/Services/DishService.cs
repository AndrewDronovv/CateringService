using CateringService.Domain.Abstractions;
using CateringService.Domain.Entities;
using CateringService.Domain.Repositories;

namespace CateringService.Application.Services;

public class DishService : BaseService<Dish>, IDishService
{
    private readonly IDishRepository _dishRepository;
    public DishService(IDishRepository dishRepository) : base(dishRepository)
    {
        _dishRepository = dishRepository;
    }
    public async Task<IEnumerable<Dish>> GetAvailableDishesAsync()
    {
        return await _dishRepository.GetAvailableDishesAsync();
    }
}