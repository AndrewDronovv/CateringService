using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.DataTransferObjects.Responses;

namespace CateringService.Application.Abstractions;

public interface IAddressService
{
    Task<AddressViewModel> CreateAddressAsync(Ulid tenantId, AddAddressRequest request);
    Task<AddressViewModel> GetByIdAsync(Ulid addresId);
    Task<AddressViewModel> UpdateAddressAsync(Ulid addressId, Ulid tenantId, UpdateAddressRequest request);
    Task<IEnumerable<AddressViewModel>> SearchAddressesByZipAsync(SearchByZipViewModel request);
    Task DeleteAddressAsync(Ulid addressId);
    Task<IEnumerable<AddressViewModel>> SearchAddressesByTextAsync(string request, CancellationToken cancellationToken);
}