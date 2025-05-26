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

    public async Task<AddressViewModel> CreateAddressAsync(AddAddressRequest request, Ulid tenantId)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request), "Address request is null.");
        }

        if (tenantId == Ulid.Empty)
        {
            throw new ArgumentException(nameof(tenantId), "TenantId is empty.");
        }

        var tenant = await _tenantRepository.GetByIdAsync(request.TenantId);

        if (tenant == null || !tenant.IsActive)
        {
            throw new Exception("Tenant is not found or not active.");
        }

        var address = _mapper.Map<Address>(request);

        var addressId = _addressRepository.Add(address);
        await _unitOfWorkRepository.SaveChangesAsync();

        var createdAddress = await _addressRepository.GetByIdAsync(addressId);

        return _mapper.Map<AddressViewModel>(createdAddress);
    }

    public async Task<AddressViewModel> GetByIdAsync(Ulid addressId)
    {
        if (addressId == Ulid.Empty)
        {
            throw new ArgumentException(nameof(addressId), "AddressId is empty.");
        }

        var address = await _addressRepository.GetByIdAsync(addressId, true);

        return _mapper.Map<AddressViewModel>(address);
    }
}
