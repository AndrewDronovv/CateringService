using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.DataTransferObjects.Responses;

namespace CateringService.Application.Abstractions;

public interface IAddressService
{
    Task<AddressViewModel> CreateAddressAsync(AddAddressRequest request, Ulid tenantId);
    Task<AddressViewModel> GetByIdAsync(Ulid addresId);
}
