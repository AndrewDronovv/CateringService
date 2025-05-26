using AutoMapper;
using CateringService.Application.Abstractions;
using CateringService.Application.DataTransferObjects.Address;
using CateringService.Domain.Entities.Approved;
using CateringService.Domain.Repositories;

namespace CateringService.Application.Services;

public class AddressService : IAddressService
{
    private readonly IAddressRepository _addressRepository;
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;
    private readonly ITenantRepository _tenantRepository;
    private readonly IMapper _mapper;

    public AddressService(IAddressRepository addressRepository, IUnitOfWorkRepository unitOfWorkRepository, ITenantRepository tenantRepository, IMapper mapper)
    {
        _addressRepository = addressRepository;
        _unitOfWorkRepository = unitOfWorkRepository;
        _tenantRepository = tenantRepository;
        _mapper = mapper;
    }

    public async Task<AddressDto> CreateAddressAsync(AddressCreateDto request, Ulid tenantId)
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
            throw new Exception("Tenant is not active or not found.");
        }

        var addressDto = _mapper.Map<Address>(request);

        var addressId = _addressRepository.Add(addressDto);

        await _unitOfWorkRepository.SaveChangesAsync();

        var createdAddress = await _addressRepository.GetByIdAsync(addressId);

        return _mapper.Map<AddressDto>(createdAddress);
    }
}
