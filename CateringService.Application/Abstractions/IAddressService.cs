using CateringService.Application.DataTransferObjects.Address;

namespace CateringService.Application.Abstractions;

public interface IAddressService
{
    Task<AddressDto> CreateAddressAsync(AddressCreateDto request, Ulid tenantId);
}
