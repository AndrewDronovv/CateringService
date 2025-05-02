using CateringService.Domain.Abstractions;
using CateringService.Domain.Entities;
using CateringService.Domain.Repositories;

namespace CateringService.Application.Services;

public class DishService : BaseService<Dish, Ulid>, IDishService
{
    private readonly IDishRepository _dishRepository;
    public DishService(IDishRepository dishRepository, IUnitOfWorkRepository unitOfWork) : 
        base(dishRepository, unitOfWork)
    {
        _dishRepository = dishRepository ?? throw new ArgumentNullException(nameof(dishRepository));
    }

    public bool CheckMenuCategoryExists(Ulid menuCategoryId)
    {
        return _dishRepository.CheckMenuCategoryExists(menuCategoryId);
    }

    public bool CheckSupplierExists(Ulid supplierId)
    {
        return _dishRepository.CheckSupplierExists(supplierId);
    }

    public async Task<bool> ToggleDishState(Ulid dishId)
    {
        var dish = await _dishRepository.GetByIdAsync(dishId);
        if (dish == null)
        {
            throw new KeyNotFoundException($"Блюдо с ID {dishId} не найдено.");
        }

        dish.IsAvailable = !dish.IsAvailable;

        var result = _dishRepository.ToggleState(dish);
        await _unitOfWork.SaveChangesAsync();

        return result;
    }

    protected override void UpdateEntity(Dish oldEntity, Dish newEntity)
    {
        if (!oldEntity.Name.Equals(newEntity.Name, StringComparison.Ordinal))
        {
            oldEntity.Name = newEntity.Name;
        }
        if (!oldEntity.Description.Equals(newEntity.Description, StringComparison.Ordinal))
        {
            oldEntity.Description = newEntity.Description;
        }
        if (oldEntity.Price != newEntity.Price)
        {
            oldEntity.Price = newEntity.Price;
        }
        if (!oldEntity.ImageUrl.Equals(newEntity.ImageUrl, StringComparison.Ordinal))
        {
            oldEntity.ImageUrl = newEntity.ImageUrl;
        }
    }
}