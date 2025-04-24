using CateringService.Domain.Abstractions;
using CateringService.Domain.Entities;
using CateringService.Domain.Repositories;

namespace CateringService.Application.Services;

public class MenuCategoryService : BaseService<MenuCategory, Ulid>, IMenuCategoryService
{
    private readonly IMenuCategoryRepository _menuCategoryRepository;
    public MenuCategoryService(IMenuCategoryRepository menuCategoryRepository, IUnitOfWorkRepository unitOfWork) : base(menuCategoryRepository, unitOfWork)
    {
        _menuCategoryRepository = menuCategoryRepository;
    }

    public Task<MenuCategory> GetByIdAndSupplierIdAsync(Ulid categoryId, Ulid supplierId)
    {
        return _menuCategoryRepository.GetByIdAndSupplierIdAsync(categoryId, supplierId);
    }

    public Task<List<MenuCategory>> GetCategoriesAsync(Ulid supplilerId)
    {
        return _menuCategoryRepository.GetBySupplierIdAsync(supplilerId);
    }
}