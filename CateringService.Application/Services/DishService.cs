using CateringService.Domain.Abstractions;
using CateringService.Domain.Entities;
using CateringService.Domain.Repositories;

namespace CateringService.Application.Services;

public class DishService : BaseService<Dish, Ulid>, IDishService
{
    private readonly IDishRepository _dishRepository;
    public DishService(IDishRepository dishRepository, IUnitOfWorkRepository unitOfWork) : base(dishRepository, unitOfWork)
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

    public async Task<IEnumerable<Dish>> GetAvailableDishesAsync()
    {
        return await _dishRepository.GetAvailableDishesAsync();
    }

    public async Task<bool> ToggleDishState(Ulid dishId, bool isAvailable)
    {
        var newDish = new Dish()
        {
            Id = dishId,
            IsAvailable = !isAvailable
        };

        var result = _dishRepository.ToggleState(newDish);
        await _unitOfWork.SaveChangesAsync();
        return result;
    }

    protected override void UpdateEntity(Dish oldEntity, Dish newEntity)
    {
        if (!oldEntity.Name.Equals(newEntity.Name, StringComparison.Ordinal))
        {
            oldEntity.Name = newEntity.Name;
        }
    }
}