using AutoMapper;
using CateringService.Application.Abstractions;
using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.DataTransferObjects.Responses;
using CateringService.Domain.Entities.Approved;
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

    public async Task<AddressViewModel?> CreateAddressAsync(AddAddressRequest request, Ulid tenantId)
    {
        if (request is null)
        {
            _logger.LogWarning("Входные данные не указаны. Операция создания адреса не может быть выполнена.");
            throw new ArgumentNullException(nameof(request), "Address request is null.");
        }

        if (tenantId == Ulid.Empty)
        {
            _logger.LogWarning("Параметр tenantId не должен быть пустым. Значение: {TenantId}", tenantId);
            throw new ArgumentException(nameof(tenantId), "TenantId is empty.");
        }

        _logger.LogInformation("Создание адреса для арендатора с Id = {TenantId}", tenantId);

        var tenant = await _tenantRepository.GetByIdAsync(request.TenantId);
        if (tenant is null || !tenant.IsActive)
        {
            _logger.LogWarning("Арендатор с Id = {request.TenantId} не найден или не активен.", request.TenantId);
            return null;
        }

        var address = _mapper.Map<Address>(request);
        if (address is null)
        {
            _logger.LogWarning("Ошибка маппинга адреса.");
            throw new InvalidOperationException("Failed to map Address.");
        }

        var addressId = _addressRepository.Add(address);
        await _unitOfWorkRepository.SaveChangesAsync();

        var createdAddress = await _addressRepository.GetByIdAsync(addressId);
        if (createdAddress is null)
        {
            _logger.LogWarning("Ошибка получения созданного адреса с Id = {AddressId}", addressId);
            return null;
        }

        _logger.LogInformation("Адрес успешно создан, страна = {Country}, город = {City}, Zip код = {Zip}",
                                createdAddress.Country, createdAddress.City, createdAddress.Zip);

        return _mapper.Map<AddressViewModel>(createdAddress);
    }

    public async Task<AddressViewModel?> GetByIdAsync(Ulid addressId)
    {
        if (addressId == Ulid.Empty)
        {
            _logger.LogWarning("Параметр addressId не должен быть пустым. Значение: {AddressId}", addressId);
            throw new ArgumentNullException(nameof(addressId), "AddressId is empty.");
        }

        _logger.LogInformation("Получение адреса с Id = {AddressId}", addressId);
        var address = await _addressRepository.GetByIdAsync(addressId);

        if (address is null)
        {
            _logger.LogWarning("Адрес с Id = {AddressId} не был найден.", addressId);
            return null;
        }

        var mappedAddress = _mapper.Map<AddressViewModel>(address);
        if (mappedAddress is null)
        {
            _logger.LogWarning("Ошибка маппинга AddressViewModel для Id = {AddressId}", addressId);
            throw new InvalidOperationException("Failed to map AddressViewModel.");
        }

        _logger.LogInformation("Адрес успешно получен: страна = {Country}, город = {City}, ZIP код = {Zip}",
                               mappedAddress.Country, mappedAddress.City, mappedAddress.Zip);

        return mappedAddress;
    }
}