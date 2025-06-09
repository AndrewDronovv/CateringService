using CateringService.Application.Abstractions;
using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.DataTransferObjects.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CateringService.Controllers;

[ApiController]
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
        Ulid tenantId = Ulid.Parse("01JXB6X43RXSAFY1G6DAT2CJWB");
        var viewModel = await _addressService.UpdateAddressAsync(addressId, tenantId, request);

        return Ok(viewModel);
    }

    //[HttpPost(ApiEndPoints.Addresses.Zip)]
    //[ProducesResponseType(typeof(AddressViewModel), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    //public async Task<ActionResult<List<AddressViewModel>>> SearchAddressesByZipAsync([FromQuery] Ulid tenantId, [FromQuery] string Zip)
    //{
    //    //return await _addressService.SearchAddressesByZipAsync();
    //}
}