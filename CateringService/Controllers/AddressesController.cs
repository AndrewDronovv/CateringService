using CateringService.Application.Abstractions;
using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.DataTransferObjects.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CateringService.Controllers
{
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly IAddressService _addressService;
        public AddressesController(IAddressService addressService) => _addressService = addressService;

        [HttpPost(ApiEndPoints.Addresses.Create)]
        [ProducesResponseType(typeof(AddressViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AddressViewModel>> CreateAddressAsync([FromBody] AddAddressRequest request)
        {
            var createdAddress = await _addressService.CreateAddressAsync(request, Ulid.NewUlid());
            if (createdAddress is null)
            {
                return NotFound(new { Message = "Арендатор не найден или не активен, или ошибка при создании адреса." });
            }

            return CreatedAtRoute("GetAddressById",
            new
            {
                addressId = createdAddress.Id
            },
            createdAddress);
        }

        [HttpGet(ApiEndPoints.Addresses.Get, Name = "GetAddressById")]
        [ProducesResponseType(typeof(AddressViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AddressViewModel>> GetAddressAsync(Ulid addressId)
        {
            var address = await _addressService.GetByIdAsync(addressId);
            if (address is null)
            {
                return NotFound(new { Message = $"Адрес с Id = {addressId} не найден." });
            }

            return Ok(address);
        }
    }
}