using AutoMapper;
using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.DataTransferObjects.Responses;
using CateringService.Domain.Abstractions;
using CateringService.Domain.Entities.Approved;
using CateringService.Domain.Repositories;
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
        _dishRepository = dishRepository;
        _supplierRepository = supplierRepository;
        _unitOfWorkRepository = unitOfWorkRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<DishViewModel?> CreateDishAsync(AddDishRequest request, Ulid supplierId)
    {
        if (request is null)
        {
            _logger.LogWarning($"Входные данные не указаны. Операция создания блюда не может быть выполнена.");
            throw new ArgumentNullException(nameof(request), "Dish request is null.");
        }

        if (supplierId == Ulid.Empty)
        {
            _logger.LogWarning("SupplierId не должен быть пустым.");
            throw new ArgumentException(nameof(supplierId), "SupplierId is empty.");
        }

        _logger.LogInformation($"Создание блюда для поставщика с Id = {supplierId}");
        var supplier = await _supplierRepository.GetByIdAsync(supplierId);
        if (supplier is null)
        {
            _logger.LogWarning($"Supplier with Id = {supplierId} was not found.");
            return null;
        }

        var dish = _mapper.Map<Dish>(request);
        if (dish is null)
        {
            _logger.LogWarning("Ошибка маппинга блюда.");
            throw new InvalidOperationException("Failed to map Dish.");
        }

        var dishId = _dishRepository.Add(dish);
        await _unitOfWorkRepository.SaveChangesAsync();

        var createdDish = await _dishRepository.GetByIdAsync(dishId);
        if (createdDish is null)
        {
            _logger.LogWarning($"Ошибка получения созданного блюда с Id = {dishId}");
            return null;
        }

        _logger.LogInformation("Блюдо {Name} успешно создано", createdDish.Name);

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