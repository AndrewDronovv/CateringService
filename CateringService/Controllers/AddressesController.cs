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

    /// <summary>
    /// Creates a new address for the specified tenant.
    /// </summary>
    /// <remarks>
    /// This endpoint allows an authenticated client to create a new address record associated with a tenant.
    /// 
    /// <b>Request Requirements:</b>
    /// - Valid <c>tenantId</c> in the payload that maps to an existing active tenant
    /// - Complete and properly formatted <see cref="AddAddressRequest"/> object in the request body
    /// 
    /// <b>Response Codes:</b>
    /// - <c>201 Created</c>: Address successfully created; returns <see cref="AddressViewModel"/>
    /// - <c>400 Bad Request</c>: Invalid input data or missing request body
    /// - <c>404 Not Found</c>: Tenant not found or inactive; address creation failed
    /// 
    /// <b>Example:</b>
    /// <code>
    /// POST /api/addresses
    /// Content-Type: application/json
    /// {
    ///   "street": "123 Main Street",
    ///   "city": "Springfield",
    ///   "postalCode": "12345",
    ///   "tenantId": "01HXYDZ4W1BGT3XYAZ2RG5CTJZ"
    /// }
    /// </code>
    /// </remarks>
    /// <param name="request">Data required to create a new address</param>
    /// <returns>Returns the created <see cref="AddressViewModel"/> with its assigned ID</returns>

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
    public async Task<ActionResult<IEnumerable<AddressViewModel>>> SearchAddressesByTextAsync(
        [FromQuery] string query, CancellationToken cancellationToken)
    {
        var addresses = await _addressService.SearchAddressesByTextAsync(query, cancellationToken);

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