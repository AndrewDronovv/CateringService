using AutoMapper;
using CateringService.Application.DataTransferObjects.Responses;
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

    public MenuCategoryService(IMenuCategoryRepository menuCategoryRepository, IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper, ILogger<MenuCategoryService> logger)
    {
        _menuCategoryRepository = menuCategoryRepository ?? throw new ArgumentNullException(nameof(menuCategoryRepository));
        _unitOfWorkRepository = unitOfWorkRepository ?? throw new ArgumentNullException(nameof(unitOfWorkRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task DeleteCategoryAsync(Ulid categoryId, Ulid supplierId)
    {
        if (await _menuCategoryRepository.HasDishesAsync(categoryId))
        {
            throw new Exception($"Нельзя удалить категорию меню так как в ней есть блюда");
        }

        await _menuCategoryRepository.DeleteAsync(categoryId, supplierId);
        _unitOfWorkRepository.SaveChanges();
    }

    public Task<MenuCategory> GetByIdAndSupplierIdAsync(Ulid categoryId, Ulid supplierId)
    {
        return _menuCategoryRepository.GetByIdAndSupplierIdAsync(categoryId, supplierId);
    }

    public Task<List<MenuCategory>> GetCategoriesAsync(Ulid supplilerId)
    {
        return _menuCategoryRepository.GetBySupplierIdAsync(supplilerId);
    }

    public async Task<List<MenuCategoryViewModel>> GetMenuCategoriesAsync(Ulid supplierId)
    {
        if (supplierId == Ulid.Empty)
        {
            _logger.LogWarning($"SupplierId не должен быть пустым.");
            throw new ArgumentException(nameof(supplierId), "SupplierId is empty.");
        }

        _logger.LogInformation($"Получение категорий меню поставщика с Id = {supplierId}");
        var menuCategories = await _menuCategoryRepository.GetBySupplierIdAsync(supplierId);

        if (menuCategories is null)
        {
            _logger.LogWarning($"Категории меню у поставщика с Id = {supplierId} не найдены.");
            return null;
        }

        var mappedMenuCategories = _mapper.Map<List<MenuCategoryViewModel>>(menuCategories);
        if (mappedMenuCategories is null)
        {
            _logger.LogWarning($"Ошибка маппинга {nameof(MenuCategoryViewModel)} для Id = {supplierId}");
            throw new InvalidOperationException($"Failed to map {nameof(MenuCategoryViewModel)} for Id = {supplierId}");
        }

        return mappedMenuCategories;
    }

    //protected override void UpdateEntity(MenuCategory oldEntity, MenuCategory newEntity)
    //{
    //    if (!oldEntity.Name.Equals(newEntity.Name, StringComparison.Ordinal))
    //    {
    //        oldEntity.Name = newEntity.Name;
    //    }

    //    if (!oldEntity.Description.Equals(newEntity.Description, StringComparison.Ordinal))
    //    {
    //        oldEntity.Description = newEntity.Description;
    //    }
    //}
}