using AutoMapper;
using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.DataTransferObjects.Responses;
using CateringService.Domain.Abstractions;
using CateringService.Domain.Entities;
using CateringService.Domain.Entities.Approved;
using CateringService.Domain.Repositories;
using CateringService.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CateringService.Application.Services;

public class DishService : IDishService
{
    private readonly IDishRepository _dishRepository;
    private readonly ISupplierRepository _supplierRepository;
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<DishService> _logger;

    public DishService(IDishRepository dishRepository, ISupplierRepository supplierRepository, IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper, ILogger<DishService> logger)
    {
        _dishRepository = dishRepository ?? throw new ArgumentNullException(nameof(dishRepository));
        _supplierRepository = supplierRepository ?? throw new ArgumentNullException(nameof(supplierRepository));
        _unitOfWorkRepository = unitOfWorkRepository ?? throw new ArgumentNullException(nameof(unitOfWorkRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<DishViewModel?> CreateDishAsync(AddDishRequest request, Ulid supplierId)
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

        var supplier = await _supplierRepository.GetByIdAsync(supplierId);
        if (supplier is null)
        {
            _logger.LogWarning("Поставщик {SupplierId} не найден.", supplierId);
            return null;
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

    public bool CheckMenuCategoryExists(Ulid menuCategoryId)
    {
        if (menuCategoryId == Ulid.Empty)
        {
            throw new ArgumentException(nameof(menuCategoryId), "MenuCategoryId is empty.");
        }

        return _dishRepository.CheckMenuCategoryExists(menuCategoryId);
    }

    public bool CheckSupplierExists(Ulid supplierId)
    {
        if (supplierId == Ulid.Empty)
        {
            throw new ArgumentException(nameof(supplierId), "SupplierId is empty.");
        }

        return _dishRepository.CheckSupplierExists(supplierId);
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
            _logger.LogWarning("Параметр dishId не должен быть пустым. Значение: {DishId}", dishId);
            throw new ArgumentNullException(nameof(dishId), "DishId is empty.");
        }

        _logger.LogInformation("Получение блюда с Id = {DishId}", dishId);

        var dish = await _dishRepository.GetByIdAsync(dishId);
        if (dish is null)
        {
            _logger.LogWarning("Блюдо с Id = {DishId} не было найдено.", dishId);
            return null;
        }

        var mappedDish = _mapper.Map<DishViewModel>(dish);
        if (mappedDish is null)
        {
            _logger.LogWarning("Ошибка маппинга DishViewModel для блюда с Id = {DishId}.", dishId);
            throw new InvalidOperationException("Failed to map DishViewModel.");
        }

        _logger.LogInformation("Получено блюдо {Name}.", mappedDish.Name);

        return mappedDish;
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