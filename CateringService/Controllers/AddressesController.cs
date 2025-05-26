using AutoMapper;
using CateringService.Application.Abstractions;
using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.DataTransferObjects.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CateringService.Controllers
{
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAddressService _addressService;

        public AddressesController(IMapper mapper, IAddressService addressService)
        {
            _mapper = mapper;
            _addressService = addressService;
        }

        [HttpPost(ApiEndPoints.Addresses.Create)]
        [ProducesResponseType(typeof(AddressViewModel), StatusCodes.Status201Created)]
        public async Task<ActionResult<AddressViewModel>> CreateAddressAsync([FromBody] AddAddressRequest input)
        {
            var createdAddress = await _addressService.CreateAddressAsync(input, Ulid.NewUlid());

            return Ok(createdAddress);
        }

        [HttpGet(ApiEndPoints.Addresses.Get)]
        [ProducesResponseType(typeof(AddressViewModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<AddressViewModel>> GetAddressAsync(Ulid addressId)
        {
            var address = await _addressService.GetByIdAsync(addressId);

            return Ok(address);
        }
    }
}
