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

public class DishService : IDishService
{
    private readonly IDishRepository _dishRepository;
    private readonly ISupplierRepository _supplierRepository;
    private readonly IMenuCategoryRepository _menuCategoryRepository;
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<DishService> _logger;

    public DishService(IDishRepository dishRepository, ISupplierRepository supplierRepository, IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper, ILogger<DishService> logger, IMenuCategoryRepository menuCategoryRepository)
    {
        _dishRepository = dishRepository ?? throw new ArgumentNullException(nameof(dishRepository));
        _supplierRepository = supplierRepository ?? throw new ArgumentNullException(nameof(supplierRepository));
        _menuCategoryRepository = menuCategoryRepository ?? throw new ArgumentNullException(nameof(menuCategoryRepository));
        _unitOfWorkRepository = unitOfWorkRepository ?? throw new ArgumentNullException(nameof(unitOfWorkRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

        var menuCategoryExists = await _menuCategoryRepository.ChechMenuCategoryExists(request.MenuCategoryId);
        if (!menuCategoryExists)
        {
            _logger.LogWarning("Категория меню {MenuCategoryId} не найдена.", request.MenuCategoryId);
            throw new NotFoundException(nameof(MenuCategory), request.MenuCategoryId.ToString());
        }

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
        var dish = await _dishRepository.GetByIdAsync(dishId);
        if (dish == null)
        {
            throw new KeyNotFoundException($"Блюдо с Id {dishId} не найдено.");
        }

        dish.IsAvailable = !dish.IsAvailable;

        var result = _dishRepository.ToggleState(dish);
        await _unitOfWorkRepository.SaveChangesAsync();

        return result;
    }

    //public async Task<List<DishViewModel?>> GetAllByIdAsync(Ulid supplierId)
    //{
    //    if (supplierId == Ulid.Empty)
    //    {
    //        _logger.LogWarning("Параметр supplierId не должен быть пустым. Значение: {SupplierId}", supplierId);
    //        throw new ArgumentNullException(nameof(supplierId), "SupplierId is empty.");
    //    }

    //    _logger.LogInformation("Получение блюда у поставщика с Id = {SupplierId}", supplierId);
    //    var dish = await _dishRepository.GetByIdAsync(supplierId);

    //    if (dish is null)
    //    {
    //        _logger.LogWarning("Блюдо у поставщика с Id = {SupplierId} не было найдено.", supplierId);
    //        return null;
    //    }

    //    var mappedDish = _mapper.Map<List<DishViewModel>>(dish);
    //    if (mappedDish is null)
    //    {
    //        _logger.LogWarning("Ошибка маппинга DishViewModel для поставщика с Id = {SupplierId}", supplierId);
    //        throw new InvalidOperationException("Failed to map AddressViewModel");
    //    }

    //    _logger.LogInformation("Получено {Count} блюд.", mappedDish.Count);

    //    return mappedDish ?? Enumerable.Empty<DishViewModel>().ToList();
    //}

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

    //protected override void UpdateEntity(Dish oldEntity, Dish newEntity)
    //{
    //    if (!string.Equals(oldEntity.Name, newEntity.Name, StringComparison.Ordinal))
    //    {
    //        oldEntity.Name = newEntity.Name;
    //    }
    //    if (!string.Equals(oldEntity.Description, newEntity.Description, StringComparison.Ordinal))
    //    {
    //        oldEntity.Description = newEntity.Description;
    //    }
    //    if (oldEntity.Price != newEntity.Price)
    //    {
    //        oldEntity.Price = newEntity.Price;
    //    }
    //    if (!string.Equals(oldEntity.ImageUrl, newEntity.ImageUrl, StringComparison.Ordinal))
    //    {
    //        oldEntity.ImageUrl = newEntity.ImageUrl;
    //    }
    //}
}