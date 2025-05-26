using AutoMapper;
using CateringService.Application.Abstractions;
using CateringService.Application.DataTransferObjects.Address;
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
        public async Task<IActionResult> CreateAddressAsync([FromBody] AddressCreateDto input)
        {
            var model = await _addressService.CreateAddressAsync(input, Ulid.NewUlid());

            return Ok(model);
        }
    }
}
