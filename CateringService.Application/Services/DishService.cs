using AutoMapper;
using CateringService.Application.Abstractions;
using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.DataTransferObjects.Responses;
using CateringService.Domain.Abstractions;
using CateringService.Domain.Entities;
using CateringService.Domain.Exceptions;
using CateringService.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace CateringService.Application.Services;

public class DishService : IDishService
{
    private readonly IDishRepository _dishRepository;
    private readonly ISupplierRepository _supplierRepository;
    private readonly IMenuCategoryRepository _menuCategoryRepository;
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<DishService> _logger;
    private readonly ISlugService _slugService;

    public DishService(IDishRepository dishRepository, ISupplierRepository supplierRepository, IUnitOfWorkRepository unitOfWorkRepository,
        IMapper mapper, ILogger<DishService> logger, IMenuCategoryRepository menuCategoryRepository, ISlugService slugService)
    {
        _dishRepository = dishRepository ?? throw new ArgumentNullException(nameof(dishRepository));
        _supplierRepository = supplierRepository ?? throw new ArgumentNullException(nameof(supplierRepository));
        _menuCategoryRepository = menuCategoryRepository ?? throw new ArgumentNullException(nameof(menuCategoryRepository));
        _unitOfWorkRepository = unitOfWorkRepository ?? throw new ArgumentNullException(nameof(unitOfWorkRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _slugService = slugService ?? throw new ArgumentNullException(nameof(slugService));
    }

    public async Task<DishViewModel?> CreateDishAsync(Ulid supplierId, AddDishRequest request)
    {
        if (request is null)
        {
            _logger.LogWarning("Входные данные не указаны.");
            throw new ArgumentNullException(nameof(request), "Dish request is null.");
        }

        if (supplierId == Ulid.Empty)
        {
            _logger.LogWarning("SupplierId не должен быть пустым.");
            throw new ArgumentException(nameof(supplierId), "SupplierId is empty.");
        }

        _logger.LogInformation("Создание блюда. Поставщик: {SupplierId}, Название: {DishName}.", supplierId, request?.Name);

        var supplierExists = await _supplierRepository.CheckSupplierExists(supplierId);
        if (!supplierExists)
        {
            _logger.LogWarning("Поставщик {SupplierId} не найден.", supplierId);
            throw new NotFoundException(nameof(Supplier), supplierId.ToString());
        }

        var menuCategoryExists = await _menuCategoryRepository.ChechMenuCategoryExists(request!.MenuCategoryId);
        if (!menuCategoryExists)
        {
            _logger.LogWarning("Категория меню {MenuCategoryId} не найдена.", request.MenuCategoryId);
            throw new NotFoundException(nameof(MenuCategory), request.MenuCategoryId.ToString());
        }

        request.Slug = _slugService.GenerateSlug(request.Name);

        var dish = _mapper.Map<Dish>(request) ?? throw new InvalidOperationException("Ошибка маппинга блюда.");

        var dishId = _dishRepository.Add(dish);
        await _unitOfWorkRepository.SaveChangesAsync();

        var createdDish = await _dishRepository.GetByIdAsync(dishId);
        if (createdDish is null)
        {
            _logger.LogWarning("Ошибка получения блюда {DishId}.", dishId);
            return null;
        }

        _logger.LogInformation("Блюдо {Name} успешно создано.", createdDish.Name);

        return _mapper.Map<DishViewModel>(createdDish);
    }

    public async Task<bool> ToggleDishStateAsync(Ulid dishId)
    {
        if (dishId == Ulid.Empty)
        {
            _logger.LogWarning("DishId не должен быть пустым.");
            throw new ArgumentException(nameof(dishId), "DishId is empty.");
        }

        var dish = await _dishRepository.GetByIdAsync(dishId);
        if (dish is null)
        {
            _logger.LogWarning("Блюдо {DishId} не найдено.", dishId);
            throw new NotFoundException(nameof(dishId), dishId.ToString());
        }

        dish.IsAvailable = !dish.IsAvailable;

        var result = _dishRepository.ToggleState(dish);
        await _unitOfWorkRepository.SaveChangesAsync();

        return result;
    }

    public async Task<DishViewModel?> GetByIdAsync(Ulid dishId)
    {
        if (dishId == Ulid.Empty)
        {
            _logger.LogWarning("DishId не должен быть пустым.");
            throw new ArgumentException(nameof(dishId), "DishId is empty.");
        }

        _logger.LogInformation("Получен запрос на блюдо {DishId}.", dishId);

        var dish = await _dishRepository.GetByIdAsync(dishId);
        if (dish is null)
        {
            _logger.LogWarning("Блюдо {DishId} не найдено.", dishId);
            throw new NotFoundException(nameof(Dish), dishId.ToString());
        }

        _logger.LogInformation("Блюдо {Name} с Id {Id} успешно получено.", dish.Name, dish.Id);

        return _mapper.Map<DishViewModel>(dish) ?? throw new InvalidOperationException("DishViewModel mapping failed.");
    }

    public async Task<DishViewModel> GetBySlugAsync(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            _logger.LogWarning("Slug не должен быть пустым.");
            throw new ArgumentException(nameof(slug), "Slug is empty.");
        }

        var normalized = _slugService.GenerateSlug(slug);
        _logger.LogInformation("Поиск блюда по slug {Normalized}", normalized);

        var dish = await _dishRepository.GetBySlugAsync(normalized);
        if (dish is null)
        {
            _logger.LogWarning("Блюдо со slug {Normalized} не найдено.", normalized);
            throw new NotFoundException(nameof(Dish), normalized);
        }

        return _mapper.Map<DishViewModel>(dish) ?? throw new InvalidOperationException("DishViewModel mapping failed.");
    }

    public async Task<IEnumerable<DishViewModel>> GetDishesBySupplierIdAsync(Ulid supplierId)
    {
        if (supplierId == Ulid.Empty)
        {
            _logger.LogWarning("SupplierId не должен быть пустым.");
            throw new ArgumentException(nameof(supplierId), "SupplierId is empty.");
        }

        _logger.LogInformation("Получен запрос на список блюд у поставщика {SupplierId}.", supplierId);

        var dishes = await _dishRepository.GetDishesBySupplierIdAsync(supplierId);
        if (!dishes.Any())
        {
            _logger.LogWarning("Список блюд у поставщика {SupplierId} пуст.", supplierId);
            return new List<DishViewModel>();
        }

        _logger.LogInformation("Получено {Count} блюд у поставщика {SupplierId}.", dishes.ToList().Count, supplierId);

        return _mapper.Map<List<DishViewModel>>(dishes);
    }
}