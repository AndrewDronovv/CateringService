using AutoMapper;
using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.DataTransferObjects.Responses;
using CateringService.Domain.Abstractions;
using CateringService.Domain.Entities;
using CateringService.Domain.Entities.Approved;
using CateringService.Domain.Exceptions;
using CateringService.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace CateringService.Application.Services;

public class MenuCategoryService : IMenuCategoryService
{
    private readonly IMenuCategoryRepository _menuCategoryRepository;
    private readonly ISupplierRepository _supplierRepository;
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;
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

    public async Task<MenuCategoryViewModel?> CreateMenuCategoryAsync(AddMenuCategoryRequest request)
    {
        if (request.SupplierId == Ulid.Empty)
        {
            _logger.LogWarning("SupplierId не должен быть пустым.");
            throw new ArgumentException(nameof(request.SupplierId), "SupplierId is empty.");
        }

        if (request is null)
        {
            _logger.LogWarning("Входные данные не указаны.");
            throw new ArgumentNullException(nameof(request), "MenuCategory request is null.");
        }

        _logger.LogInformation("Создание категории меню. Поставщик {SupplierId}, Название {Name}.", request.SupplierId, request?.Name);

        var supplierExists = await _supplierRepository.CheckSupplierExists(request.SupplierId);
        if (!supplierExists)
        {
            _logger.LogWarning("Поставщик {SupplierId} не найден.", request.SupplierId);
            throw new NotFoundException(nameof(Supplier), request.SupplierId.ToString());
        }

        var menuCategory = _mapper.Map<MenuCategory>(request) ?? throw new InvalidOperationException("MenuCategory mapping failed.");

        var menuCategoryId = _menuCategoryRepository.Add(menuCategory);
        await _unitOfWorkRepository.SaveChangesAsync();

        var createdMenuCategory = await _menuCategoryRepository.GetByIdAsync(menuCategoryId);
        if (createdMenuCategory is null)
        {
            _logger.LogWarning("Ошибка получения категории меню {MenuCategoryId}.", menuCategoryId);
            throw new NotFoundException(nameof(MenuCategory), menuCategoryId.ToString());
        }

        _logger.LogInformation("Категория меню {Name} успешно создана.", createdMenuCategory.Name);

        return _mapper.Map<MenuCategoryViewModel>(createdMenuCategory);
    }

    public async Task DeleteMenuCategoryAsync(Ulid menuCategoryId, Ulid supplierId)
    {
        if (menuCategoryId == Ulid.Empty)
        {
            _logger.LogWarning("MenuCategoryId не должен быть пустым.");
            throw new ArgumentException(nameof(menuCategoryId), "MenuCategoryId is empty.");
        }

        if (supplierId == Ulid.Empty)
        {
            _logger.LogWarning("SupplierId не должен быть пустым.");
            throw new ArgumentException(nameof(supplierId), "SupplierId is empty.");
        }

        _logger.LogInformation("Получен запрос на удаление категории меню {MenuCategoryId} у поставщика {SupplierId}.", menuCategoryId, supplierId);

        var supplierExists = await _supplierRepository.CheckSupplierExists(supplierId);
        if (!supplierExists)
        {
            _logger.LogWarning("Поставщик {SupplierId} не найден.", supplierId);
            throw new NotFoundException(nameof(Supplier), supplierId.ToString());
        }

        if (await _menuCategoryRepository.HasDishesAsync(menuCategoryId))
        {
            _logger.LogWarning("Нельзя удалить категорию меню {MenuCategoryId} так как в ней есть блюда.", menuCategoryId);
            throw new ArgumentException($"Нельзя удалить категорию меню так как в ней есть блюда.");
        }

        await _menuCategoryRepository.DeleteAsync(menuCategoryId, supplierId);
        _logger.LogInformation("Категория меню {MenuCategoryId} у поставщика {SupplierId} удалена.", menuCategoryId, supplierId);
        _unitOfWorkRepository.SaveChanges();
    }

    public async Task<MenuCategoryViewModel> GetMenuCategoryBySupplierIdAsync(Ulid menuCategoryId, Ulid supplierId)
    {
        if (menuCategoryId == Ulid.Empty)
        {
            _logger.LogWarning("MenuCategoryId не должен быть пустым.");
            throw new ArgumentException(nameof(menuCategoryId), "MenuCategoryId is empty.");
        }

        if (supplierId == Ulid.Empty)
        {
            _logger.LogWarning("SupplierId не должен быть пустым.");
            throw new ArgumentException(nameof(supplierId), "SupplierId is empty.");
        }

        _logger.LogInformation("Получен запрос на категорию меню {MenuCategoryId} у поставщика {SupplierId}.", menuCategoryId, supplierId);

        var supplierExists = await _supplierRepository.CheckSupplierExists(supplierId);
        if (!supplierExists)
        {
            _logger.LogWarning("Поставщик {SupplierId} не найден.", supplierId);
            throw new NotFoundException(nameof(Supplier), supplierId.ToString());
        }

        MenuCategory? menuCategoryCurrent = await _menuCategoryRepository.GetByIdAsync(menuCategoryId);
        if (menuCategoryCurrent is null)
        {
            _logger.LogWarning("Категория меню {MenuCategoryId} не найдена.", menuCategoryId);
            throw new NotFoundException(nameof(MenuCategory), menuCategoryId.ToString());
        }

        var menuCategory = await _menuCategoryRepository.GetMenuCategoryBySupplierIdAsync(menuCategoryId, supplierId);

        return _mapper.Map<MenuCategoryViewModel>(menuCategory) ?? throw new InvalidOperationException("MenuCategoryViewModel mapping failed.");
    }

    public async Task<List<MenuCategoryViewModel>> GetMenuCategoriesAsync(Ulid supplierId)
    {
        if (supplierId == Ulid.Empty)
        {
            _logger.LogWarning("SupplierId не должен быть пустым.");
            throw new ArgumentException(nameof(supplierId), "SupplierId is empty.");
        }

        var supplierExists = await _supplierRepository.CheckSupplierExists(supplierId);
        if (!supplierExists)
        {
            _logger.LogWarning("Поставщик {SupplierId} не найден.", supplierId);
            throw new NotFoundException(nameof(Supplier), supplierId.ToString());
        }

        _logger.LogInformation("Получен запрос на категории меню для поставщика {SupplierId}", supplierId);

        var menuCategories = await _menuCategoryRepository.GetBySupplierIdAsync(supplierId);
        _logger.LogInformation("Найдено {Count} категорий меню.", menuCategories.Count);

        return _mapper.Map<List<MenuCategoryViewModel>>(menuCategories) ?? new List<MenuCategoryViewModel>();
    }

    public async Task<MenuCategoryViewModel> UpdateMenuCategoryAsync(Ulid menuCategoryId, Ulid supplierId, UpdateMenuCategoryRequest request)
    {
        if (supplierId == Ulid.Empty)
        {
            _logger.LogWarning("SupplierId не должен быть пустым.");
            throw new ArgumentException(nameof(supplierId), "SupplierId is empty.");
        }

        if (menuCategoryId == Ulid.Empty)
        {
            _logger.LogWarning("MenuCategoryId не должен быть пустым.");
            throw new ArgumentException(nameof(menuCategoryId), "MenuCategoryId is empty.");
        }

        var supplierExists = await _supplierRepository.CheckSupplierExists(supplierId);
        if (!supplierExists)
        {
            _logger.LogWarning("Поставщик {SupplierId} не найден.", supplierId);
            throw new NotFoundException(nameof(Supplier), supplierId.ToString());
        }

        MenuCategory? menuCategoryCurrent = await _menuCategoryRepository.GetByIdAsync(menuCategoryId);
        if (menuCategoryCurrent is null)
        {
            _logger.LogWarning("Категория меню {MenuCategoryId} не найдена.", menuCategoryId);
            throw new NotFoundException(nameof(Supplier), supplierId.ToString());
        }

        _mapper.Map(request, menuCategoryCurrent);
        await _unitOfWorkRepository.SaveChangesAsync();

        return _mapper.Map<MenuCategoryViewModel>(menuCategoryCurrent) ?? throw new InvalidOperationException("MenuCategoryViewModel mapping failed.");
    }
}