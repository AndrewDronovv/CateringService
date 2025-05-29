using AutoMapper;
using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.DataTransferObjects.Responses;
using CateringService.Domain.Abstractions;
using CateringService.Domain.Entities;
using CateringService.Domain.Entities.Approved;
using CateringService.Domain.Repositories;
using CateringService.Persistence.Repositories;
using Microsoft.Extensions.Logging;

namespace CateringService.Application.Services;

public class MenuCategoryService : IMenuCategoryService
{
    private readonly IMenuCategoryRepository _menuCategoryRepository;
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;
    private readonly ISupplierRepository _supplierRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<MenuCategoryService> _logger;

    public MenuCategoryService(IMenuCategoryRepository menuCategoryRepository, ISupplierRepository supplierRepository, IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper, ILogger<MenuCategoryService> logger)
    {
        _menuCategoryRepository = menuCategoryRepository ?? throw new ArgumentNullException(nameof(menuCategoryRepository));
        _supplierRepository = supplierRepository ?? throw new ArgumentNullException(nameof(supplierRepository));
        _unitOfWorkRepository = unitOfWorkRepository ?? throw new ArgumentNullException(nameof(unitOfWorkRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<MenuCategoryViewModel?> CreateMenuCategoryAsync(AddMenuCategoryRequest request, Ulid supplierId)
    {
        if (request is null)
        {
            _logger.LogWarning("Входные данные не указаны.");
            throw new ArgumentNullException(nameof(request), "MenuCategory request is null.");
        }

        if (supplierId == Ulid.Empty)
        {
            _logger.LogWarning("SupplierId не должен быть пустым.");
            return null;
        }

        _logger.LogInformation("Создание категори меню. Поставщик: {SupplierId}, Название: {Name}.", supplierId, request?.Name);

        var supplier = await _supplierRepository.GetByIdAsync(supplierId);
        if (supplier is null)
        {
            _logger.LogWarning("Поставщик {SupplierId} не найден.", supplierId);
            return null;
        }

        var menuCategory = _mapper.Map<MenuCategory>(request) ?? throw new InvalidOperationException("Ошибка маппинга категории меню.");

        var menuCategoryId = _menuCategoryRepository.Add(menuCategory);
        await _unitOfWorkRepository.SaveChangesAsync();

        var createdMenuCategory = await _menuCategoryRepository.GetByIdAsync(menuCategoryId);
        if (createdMenuCategory is null)
        {
            _logger.LogWarning("Ошибка получения категории меню {MenuCategoryId}.", menuCategoryId);
            return null;
        }

        _logger.LogInformation("Категория меню {Name} успешно создана.", createdMenuCategory.Name);

        return _mapper.Map<MenuCategoryViewModel>(createdMenuCategory);
    }

    public async Task DeleteCategoryAsync(Ulid categoryId, Ulid supplierId)
    {
        if (await _menuCategoryRepository.HasDishesAsync(categoryId))
        {
            throw new Exception($"Нельзя удалить категорию меню так как в ней есть блюда.");
        }

        await _menuCategoryRepository.DeleteAsync(categoryId, supplierId);
        _unitOfWorkRepository.SaveChanges();
    }

    public async Task<MenuCategoryViewModel> GetByIdAndSupplierIdAsync(Ulid supplierId, Ulid menuCategoryId)
    {
        if (supplierId == Ulid.Empty || menuCategoryId == Ulid.Empty)
        {
            _logger.LogWarning("Некорректные параметры SupplierId = {SupplierId}, MenuCategoryId = {MenuCategoryId}", supplierId, menuCategoryId);
            throw new ArgumentException("SupplierId и MenuCategoryId должны быть заполнены.");
        }

        _logger.LogInformation("Получен запрос на категорию меню {MenuCategoryId} у поставщика {SupplierId}.", menuCategoryId, supplierId);

        var menuCategory = await _menuCategoryRepository.GetByIdAndSupplierIdAsync(supplierId, menuCategoryId);

        return _mapper.Map<MenuCategoryViewModel>(menuCategory) ?? throw new InvalidOperationException("Ошибка маппинга MenuCategoryViewModel.");
    }

    public async Task<List<MenuCategoryViewModel>> GetMenuCategoriesAsync(Ulid supplierId)
    {
        if (supplierId == Ulid.Empty)
        {
            _logger.LogWarning("SupplierId не должен быть пустым.");
            throw new ArgumentException(nameof(supplierId), "SupplierId is empty.");
        }

        _logger.LogInformation("Получен запрос на категории меню для поставщика {SupplierId}", supplierId);

        var menuCategories = await _menuCategoryRepository.GetBySupplierIdAsync(supplierId);

        return _mapper.Map<List<MenuCategoryViewModel>>(menuCategories) ?? new List<MenuCategoryViewModel>();
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