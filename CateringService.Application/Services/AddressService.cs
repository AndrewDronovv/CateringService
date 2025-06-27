using AutoMapper;
using CateringService.Application.Abstractions;
using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.DataTransferObjects.Responses;
using CateringService.Domain.Entities;
using CateringService.Domain.Entities.Approved;
using CateringService.Domain.Exceptions;
using CateringService.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace CateringService.Application.Services;

public class AddressService : IAddressService
{
    private readonly IAddressRepository _addressRepository;
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;
    private readonly ITenantRepository _tenantRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<AddressService> _logger;

    public AddressService(IAddressRepository addressRepository, IUnitOfWorkRepository unitOfWorkRepository, ITenantRepository tenantRepository, IMapper mapper, ILogger<AddressService> logger)
    {
        _addressRepository = addressRepository ?? throw new ArgumentNullException(nameof(addressRepository));
        _unitOfWorkRepository = unitOfWorkRepository ?? throw new ArgumentNullException(nameof(unitOfWorkRepository));
        _tenantRepository = tenantRepository ?? throw new ArgumentNullException(nameof(tenantRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<AddressViewModel?> CreateAddressAsync(Ulid tenantId, AddAddressRequest request)
    {
        if (request is null)
        {
            _logger.LogWarning("Входные данные не указаны.");
            throw new ArgumentNullException(nameof(request), "Address request is null.");
        }

        if (tenantId == Ulid.Empty)
        {
            _logger.LogWarning("TenantId не должен быть пустым.");
            throw new ArgumentException(nameof(tenantId), "TenantId is empty.");
        }

        _logger.LogInformation("Создание адреса для арендатора {TenantId}", tenantId);

        var tenant = await _tenantRepository.GetByIdAsync(request.TenantId);
        if (tenant is null || !tenant.IsActive)
        {
            _logger.LogWarning("Арендатор с Id {request.TenantId} не найден или не активен.", request.TenantId);
            throw new NotFoundException(nameof(Tenant), request.TenantId.ToString());
        }

        var address = _mapper.Map<Address>(request) ?? throw new InvalidOperationException("Address mapping failed.");

        var addressId = _addressRepository.Add(address);
        await _unitOfWorkRepository.SaveChangesAsync();

        var createdAddress = await _addressRepository.GetByIdAsync(addressId);
        if (createdAddress is null)
        {
            _logger.LogWarning("Ошибка получения адреса {AddressId}.", addressId);
            throw new NotFoundException(nameof(Address), addressId.ToString());
        }

        _logger.LogInformation("Адрес успешно создан, страна = {Country}, город = {City}, Zip код = {Zip}.", createdAddress.Country, createdAddress.City, createdAddress.Zip);

        return _mapper.Map<AddressViewModel>(createdAddress);
    }

    //TODO: добавить в параметры метода Ulid requestingUserId из задания.
    public async Task DeleteAddressAsync(Ulid addressId)
    {
        if (addressId == Ulid.Empty)
        {
            _logger.LogWarning("AddressId не должен быть пустым.");
            throw new ArgumentException(nameof(addressId), "AddressId is empty.");
        }

        _logger.LogInformation("Получен запрос на удаление адреса {AddressId}.", addressId);

        var addressExists = await _addressRepository.CheckAddressExistsAsync(addressId);
        if (!addressExists)
        {
            _logger.LogWarning("Адрес {AddressId} не найден.", addressId);
            throw new NotFoundException(nameof(Address), addressId.ToString());
        }

        if (await _addressRepository.HasActiveOrdersAsync(addressId))
        {
            _logger.LogWarning("Нельзя удалить адрес {AddressId} так как по нему есть активные заказы.", addressId);
            throw new ArgumentException($"Нельзя удалить адрес так как по нему есть активные заказы.");
        }

        await _addressRepository.DeleteAsync(addressId);
        await _unitOfWorkRepository.SaveChangesAsync();

        _logger.LogInformation("Адрес {AddressId} успешно удален.", addressId);
    }

    public async Task<AddressViewModel?> GetByIdAsync(Ulid addressId)
    {
        if (addressId == Ulid.Empty)
        {
            _logger.LogWarning("AddressId не должен быть пустым.");
            throw new ArgumentException(nameof(addressId), "AddressId is empty.");
        }

        _logger.LogInformation("Получен запрос на адрес {AddressId}.", addressId);

        var address = await _addressRepository.GetByIdAsync(addressId);
        if (address is null)
        {
            _logger.LogWarning("Адрес {AddressId} не найден.", addressId);
            throw new NotFoundException(nameof(Address), addressId.ToString());
        }

        _logger.LogInformation("Адрес {AddressId} успешно получен.", addressId);

        return _mapper.Map<AddressViewModel>(address) ?? throw new InvalidOperationException("AddressViewModel mapping failed.");
    }

    public async Task<IEnumerable<AddressViewModel>> SearchAddressesByTextAsync(string query)
    {
        var addresses = await _addressRepository.SearchByTextAsync(query);

        return _mapper.Map<IEnumerable<AddressViewModel>>(addresses);
    }

    public async Task<IEnumerable<AddressViewModel>> SearchAddressesByZipAsync(SearchByZipViewModel request)
    {
        if (request is null)
        {
            _logger.LogWarning("Входные данные не указаны.");
            throw new ArgumentNullException(nameof(request), "Address request is null.");
        }

        var zipCodes = await _addressRepository.SearchByZipAsync(request.TenantId, request.Zip);

        return _mapper.Map<IEnumerable<AddressViewModel>>(zipCodes);
    }

    public async Task<AddressViewModel> UpdateAddressAsync(Ulid addressId, Ulid tenantId, UpdateAddressRequest request)
    {
        if (addressId == Ulid.Empty)
        {
            _logger.LogWarning("AddressId не должен быть пустым.");
            throw new ArgumentException(nameof(addressId), "AddressId is empty.");
        }

        if (tenantId == Ulid.Empty)
        {
            _logger.LogWarning("TenantId не должен быть пустым.");
            throw new ArgumentException(nameof(tenantId), "TenantId is empty.");
        }

        var tenantExists = await _tenantRepository.CheckActiveTenantExistsAsync(tenantId);
        if (!tenantExists)
        {
            _logger.LogWarning("Арендатор {TenantId} не найден или деактивирован.", tenantId);
            throw new NotFoundException(nameof(Tenant), tenantId.ToString());
        }

        Address addressCurrent = await _addressRepository.GetByIdAsync(addressId);
        if (addressCurrent is null)
        {
            _logger.LogWarning("Адрес {AddressId} не найден.", addressId);
            throw new NotFoundException(nameof(Address), addressId.ToString());
        }

        _mapper.Map(request, addressCurrent);

        _addressRepository.Update(addressCurrent, true);

        addressCurrent.UpdatedAt = DateTime.UtcNow;

        await _unitOfWorkRepository.SaveChangesAsync();

        return _mapper.Map<AddressViewModel>(addressCurrent) ?? throw new InvalidOperationException("AddressViewModel mapping failed.");
    }
}