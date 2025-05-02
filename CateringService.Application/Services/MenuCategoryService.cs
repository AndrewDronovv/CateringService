using CateringService.Domain.Abstractions;
using CateringService.Domain.Entities;
using CateringService.Domain.Repositories;

namespace CateringService.Application.Services;

public class MenuCategoryService : BaseService<MenuCategory, Ulid>, IMenuCategoryService
{
    private readonly IMenuCategoryRepository _menuCategoryRepository;
    public MenuCategoryService(IMenuCategoryRepository menuCategoryRepository, IUnitOfWorkRepository unitOfWork) : 
        base(menuCategoryRepository, unitOfWork)
    {
        _menuCategoryRepository = menuCategoryRepository ?? throw new ArgumentNullException(nameof(menuCategoryRepository));
    }

    public async Task DeleteCategoryAsync(Ulid categoryId, Ulid supplierId)
    {
        if (await _menuCategoryRepository.HasDishesAsync(categoryId))
        {
            throw new Exception($"Нельзя удалить категорию меню так как в ней есть блюда");
        }

        await _menuCategoryRepository.DeleteAsync(categoryId, supplierId);
        _unitOfWork.SaveChanges();
    }

    public Task<MenuCategory> GetByIdAndSupplierIdAsync(Ulid categoryId, Ulid supplierId)
    {
        return _menuCategoryRepository.GetByIdAndSupplierIdAsync(categoryId, supplierId);
    }

    public Task<List<MenuCategory>> GetCategoriesAsync(Ulid supplilerId)
    {
        return _menuCategoryRepository.GetBySupplierIdAsync(supplilerId);
    }

    protected override void UpdateEntity(MenuCategory oldEntity, MenuCategory newEntity)
    {
        if (!oldEntity.Name.Equals(newEntity.Name, StringComparison.Ordinal))
        {
            oldEntity.Name = newEntity.Name;
        }

        if (!oldEntity.Description.Equals(newEntity.Description, StringComparison.Ordinal))
        {
            oldEntity.Description = newEntity.Description;
        }
    }
}