using Asp.Versioning;
using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.DataTransferObjects.Responses;
using CateringService.Domain.Abstractions;
using CateringService.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CateringService.Controllers;

[ApiController]
[TypeFilter<LoggingActionFilter>]
[ApiVersion(1)]
[ApiVersion(2)]
[Route("api/v{version:apiVersion}")]
public class DishesController : ControllerBase
{
    private readonly IDishService _dishService;
    public DishesController(IDishService dishAppService)
    {
        _dishService = dishAppService ?? throw new ArgumentNullException(nameof(dishAppService));
    }

    [HttpGet("[controller]/by-id/{dishId}", Name = "GetDishByIdV1")]
    [MapToApiVersion(1)]
    [ProducesResponseType(typeof(DishViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DishViewModel>> GetDishByIdAsyncV1([FromRoute] Ulid dishId)
    {
        var dish = await _dishService.GetByIdAsync(dishId);

        return Ok(dish);
    }

    [HttpGet("[controller]by-id/{dishId}", Name = "GetDishByIdV2")]
    [MapToApiVersion(2)]
    [ProducesResponseType(typeof(DishViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DishViewModel>> GetDishByIdAsyncV2(Ulid dishId)
    {
        var dish = await _dishService.GetByIdAsync(dishId);

        return Ok(dish);
    }

    [HttpGet("[controller]/by-slug/{slug}")]
    [MapToApiVersion(1)]
    [ProducesResponseType(typeof(DishViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DishViewModel>> GetDishBySlugAsync(string slug)
    {
        var dish = await _dishService.GetBySlugAsync(slug);

        return Ok(dish);
    }

    [HttpPatch("[controller]/{dishId}/toggle")]
    [MapToApiVersion(1)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ToggleDishStateAsync(Ulid dishId)
    {
        await _dishService.ToggleDishStateAsync(dishId);

        return NoContent();
    }

    [HttpPost("suppliers/{supplierId}/[controller]")]
    [MapToApiVersion(1)]
    [ProducesResponseType(typeof(DishViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DishViewModel>> CreateDishAsync([FromBody] AddDishRequest request, [FromRoute] Ulid supplierId)
    {
        var createdDish = await _dishService.CreateDishAsync(supplierId, request);

        return CreatedAtRoute("GetDishByIdV1", new { dishId = createdDish.Id }, createdDish);
    }

    [HttpGet("suppliers/{supplierId}/[controller]")]
    [MapToApiVersion(1)]
    [ProducesResponseType(typeof(IEnumerable<DishViewModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DishViewModel>>> GetDishesBySupplierIdAsync(Ulid supplierId)
    {
        var dishes = await _dishService.GetDishesBySupplierIdAsync(supplierId);

        return Ok(dishes);
    }
}