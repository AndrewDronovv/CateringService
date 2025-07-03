using CateringService.Application.Abstractions;
using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.DataTransferObjects.Responses;
using CateringService.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CateringService.Controllers;

[ApiController]
[TypeFilter<LoggingActionFilter>]
public class AddressesController : ControllerBase
{
    private readonly IAddressService _addressService;
    public AddressesController(IAddressService addressService)
    {
        _addressService = addressService;
    }

    
    [HttpPost(ApiEndPoints.Addresses.Create)]
    [ProducesResponseType(typeof(AddressViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AddressViewModel>> CreateAddressAsync([FromBody] AddAddressRequest request)
    {
        var createdAddress = await _addressService.CreateAddressAsync(Ulid.NewUlid(), request);

        return CreatedAtRoute("GetAddressById", new { addressId = createdAddress.Id }, createdAddress);
    }

    [HttpGet(ApiEndPoints.Addresses.Get, Name = "GetAddressById")]
    [ProducesResponseType(typeof(AddressViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AddressViewModel>> GetAddressAsync(Ulid addressId)
    {
        var address = await _addressService.GetByIdAsync(addressId);

        return Ok(address);
    }

    [HttpPut(ApiEndPoints.Addresses.Update)]
    [ProducesResponseType(typeof(AddressViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAddressAsync(Ulid addressId, UpdateAddressRequest request)
    {
        Ulid tenantId = Ulid.Parse("01H5PY6RF4WKFCR9VCMY2QNFGP");
        var viewModel = await _addressService.UpdateAddressAsync(addressId, tenantId, request);

        return Ok(viewModel);
    }

    [HttpGet("api/addresses/search-by-zip")]
    [ProducesResponseType(typeof(IEnumerable<AddressViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<AddressViewModel>>> SearchAddressesByZipAsync([FromQuery] Ulid tenantId, [FromQuery] string Zip)
    {
        var addresses = await _addressService.SearchAddressesByZipAsync(new SearchByZipViewModel
        {
            TenantId = tenantId,
            Zip = Zip
        });

        return Ok(addresses);
    }

    [HttpGet("api/addresses/search")]
    [ProducesResponseType(typeof(IEnumerable<AddressViewModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AddressViewModel>>> SearchAddressesByTextAsync([FromQuery] string query)
    {
        var addresses = await _addressService.SearchAddressesByTextAsync(query);

        return Ok(addresses);
    }

    [HttpDelete("api/addresses/{addressId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTenantAsync(Ulid addressId)
    {
        await _addressService.DeleteAddressAsync(addressId);

        return NoContent();
    }
}