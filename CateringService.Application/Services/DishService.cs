using CateringService.Domain.Abstractions;
using CateringService.Domain.Entities;
using CateringService.Domain.Repositories;

namespace CateringService.Application.Services;

public class DishService : BaseService<Dish, Ulid>, IDishService
{
    private readonly IDishRepository _dishRepository;
    public DishService(IDishRepository dishRepository, IUnitOfWorkRepository unitOfWork) : base(dishRepository, unitOfWork)
    {
        _dishRepository = dishRepository;
    }

    public async Task<bool> CheckMenuCategoryExistsAsync(Ulid menuCategoryId)
    {
        return await _dishRepository.CheckMenuCategoryExistsAsync(menuCategoryId);
    }

    public async Task<bool> CheckSupplierExistsAsync(Ulid supplierId)
    {
        return await _dishRepository.CheckSupplierExistsAsync(supplierId);
    }

    public async Task<IEnumerable<Dish>> GetAvailableDishesAsync()
    {
        return await _dishRepository.GetAvailableDishesAsync();
    }
}