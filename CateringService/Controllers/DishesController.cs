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
[Route("api/v{version:apiVersion}/[controller]")]
public class DishesController : ControllerBase
{
    private readonly IDishService _dishService;
    public DishesController(IDishService dishAppService)
    {
        _dishService = dishAppService ?? throw new ArgumentNullException(nameof(dishAppService));
    }

    [HttpGet("by-id/{dishId}", Name = "GetDishByIdV1")]
    [MapToApiVersion(1)]
    [ProducesResponseType(typeof(DishViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DishViewModel>> GetDishByIdAsyncV1([FromRoute] Ulid dishId)
    {
        var dish = await _dishService.GetByIdAsync(dishId);

        return Ok(dish);
    }

    [HttpGet("by-id/{dishId}", Name = "GetDishByIdV2")]
    [MapToApiVersion(2)]
    [ProducesResponseType(typeof(DishViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DishViewModel>> GetDishByIdAsyncV2(Ulid dishId)
    {
        var dish = await _dishService.GetByIdAsync(dishId);

        return Ok(dish);
    }

    //[HttpGet("api/dishes/by-slug/{slug}")]
    //[ProducesResponseType(typeof(DishViewModel), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    //public async Task<ActionResult<DishViewModel>> GetDishBySlugAsync(string slug)
    //{
    //    var dish = await _dishService.GetBySlugAsync(slug);

    //    return Ok(dish);
    //}

    //[HttpPost(ApiEndPoints.Dishes.Create)]
    //[ProducesResponseType(typeof(DishViewModel), StatusCodes.Status201Created)]
    //[ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    //public async Task<ActionResult<DishViewModel>> CreateDishAsync([FromBody] AddDishRequest request, Ulid supplierId)
    //{
    //    var createdDish = await _dishService.CreateDishAsync(supplierId, request);

    //    return CreatedAtRoute("GetDishById", new { dishId = createdDish.Id }, createdDish);
    //}

    //[HttpGet("api/dishes")]
    //[ProducesResponseType(typeof(List<DishViewModel>), StatusCodes.Status200OK)]
    //public async Task<ActionResult<List<DishViewModel>>> GetDishesAsync()
    //{
    //    var dishes = await _dishService.GetDishesAsync();

    //    return dishes;
    //}

    //[HttpGet(ApiEndPoints.Dishes.GetDishes)]
    //[ProducesResponseType(typeof(IEnumerable<DishViewModel>), StatusCodes.Status200OK)]
    //public async Task<ActionResult<IEnumerable<DishViewModel>>> GetDishesBySupplierIdAsync(Ulid supplierId)
    //{
    //    var dishes = await _dishService.GetAllByIdAsync(supplierId);
    //    if (dishes is null || !dishes.Any())
    //    {
    //        _logger.LogInformation("Список блюд пуст.");
    //        return Ok(Enumerable.Empty<DishViewModel>());
    //    }

    //    var dishesDto = _mapper.Map<IEnumerable<DishViewModel>>(dishes);
    //    _logger.LogInformation($"Запрос списка блюд выполнен успешно. Найдено {dishesDto.Count()} блюд.");
    //    return Ok(dishesDto);
    //}

    //[HttpDelete(ApiEndPoints.Dishes.Delete)]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //public async Task<IActionResult> DeleteDishAsync(Ulid dishId)
    //{
    //    if (dishId == Ulid.Empty)
    //    {
    //        _logger.LogWarning($"Id не должен быть пустым.");
    //        return BadRequest(new { Error = "Id не должен быть пустым." });
    //    }

    //    var deletedDish = await _dishService.GetByIdAsync(dishId);

    //    await _dishService.DeleteAsync(dishId);
    //    _logger.LogInformation($"Блюдо {deletedDish} с Id = {dishId} успешно удалено.");
    //    return Ok(new
    //    {
    //        Success = true,
    //        Message = $"Выбранное блюдо {deletedDish?.Name} удалено",
    //        Timestamp = DateTime.Now
    //    });
    //}

    //[HttpPut(ApiEndPoints.Dishes.Update)]
    //[ProducesResponseType(StatusCodes.Status204NoContent)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //public async Task<IActionResult> UpdateDishAsync(Ulid dishId, UpdateDishRequest input)
    //{
    //    if (dishId == Ulid.Empty)
    //    {
    //        _logger.LogWarning($"Id не должен быть пустым.");
    //        return BadRequest(new { Error = "Id не должен быть пустым." });
    //    }

    //    if (input is null)
    //    {
    //        _logger.LogWarning("Входные данные не указаны. Операция обновления блюда не может быть выполнена.");
    //        return BadRequest(new { Error = "Входные параметры отсутствуют. Пожалуйста, предоставьте данные для создания блюда." });
    //    }

    //    var updateRequest = _mapper.Map<Dish>(input);

    //    await _dishService.UpdateAsync(dishId, updateRequest);

    //    _logger.LogInformation($"Блюдо с Id = {dishId} успешно обновлено.");
    //    return NoContent();
    //}

    //[HttpPatch(ApiEndPoints.Dishes.Toggle)]
    //[ProducesResponseType(StatusCodes.Status204NoContent)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //public async Task<IActionResult> ToggleDishState(Ulid dishId)
    //{
    //    var dish = await _dishService.GetByIdAsync(dishId);
    //    if (dish is null)
    //    {
    //        _logger.LogWarning($"Блюдо с Id = {dishId} не найдено.");
    //        return NotFound();
    //    }

    //    var result = await _dishService.ToggleDishStateAsync(dishId);

    //    return NoContent();
    //}
}