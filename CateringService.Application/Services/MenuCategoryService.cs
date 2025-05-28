using AutoMapper;
using CateringService.Domain.Abstractions;
using CateringService.Domain.Entities.Approved;
using CateringService.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace CateringService.Application.Services;

public class MenuCategoryService : IMenuCategoryService
{
    private readonly IMenuCategoryRepository _menuCategoryRepository;
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<MenuCategoryService> _logger;
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