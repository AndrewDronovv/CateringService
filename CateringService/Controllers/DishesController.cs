using CateringService.Application.Abstractions;
using CateringService.Application.DataTransferObjects.Dish;
using Microsoft.AspNetCore.Mvc;

namespace CateringService.Controllers;

[ApiController]
public class DishesController : ControllerBase
{
    private readonly IDishAppService _dishAppService;
    private readonly ILogger<DishesController> _logger;

    public DishesController(IDishAppService dishAppService, ILogger<DishesController> logger)
    {
        _dishAppService = dishAppService ?? throw new ArgumentNullException(nameof(dishAppService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet(ApiEndPoints.Dishes.GetAll)]
    [ProducesResponseType(typeof(IEnumerable<DishDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<DishDto>>> GetDishesAsync()
    {
        _logger.LogInformation("Получен запрос на список блюд.");
        try
        {
            var dishes = await _dishAppService.GetDishesAsync();

            if (dishes == null || !dishes.Any())
            {
                _logger.LogInformation("Список блюд пуст.");
                return Ok(Enumerable.Empty<DishDto>());
            }

            _logger.LogInformation($"Запрос списка блюд выполнен успешно. Найдено {dishes.Count()} блюд.");
            return Ok(dishes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении списка блюд.");
            return StatusCode(500, new { Error = "Произошла ошибка на сервере." });
        }
    }

    [HttpGet(ApiEndPoints.Dishes.Get, Name = "GetDishById")]
    [ProducesResponseType(typeof(DishDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<DishDto>> GetDishAsync(Ulid id)
    {
        try
        {
            _logger.LogInformation($"Получен запрос блюда с Id = {id}.");
            if (id == Ulid.Empty)
            {
                _logger.LogWarning($"Id должен быть больше 0.");
                return BadRequest(new { Error = "Id должен быть больше 0." });
            }

            var dish = await _dishAppService.GetDishByIdAsync(id);

            if (dish == null)
            {
                _logger.LogWarning($"Блюдо с Id {id} не найдено.");
                return NotFound(new { Error = $"Блюдо с Id {id} не найдено." });
            }

            _logger.LogInformation($"Блюдо с Id {id} успешно получено.");
            return Ok(dish);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении блюда по Id.");
            return StatusCode(500, new { Error = "Произошла ошибка на сервере." });
        }
    }

    [HttpPost(ApiEndPoints.Dishes.Create)]
    [ProducesResponseType(typeof(DishCreateDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DishDto>> CreateDishAsync([FromBody] DishCreateDto input)
    {
        try
        {
            if (input is null)
            {
                _logger.LogWarning("CreateDishDto равен null");
                return BadRequest(new { Error = "CreateDishDto равен null" });
            }
            
            var createdDish = await _dishAppService.CreateDishAsync(input);

            return CreatedAtRoute("GetDishById",
                new
                {
                    id = createdDish.Id
                },
                createdDish);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при создании блюда.");
            return StatusCode(500, new { Error = "Произошла ошибка на сервере" });
        }
    }

    [HttpDelete(ApiEndPoints.Dishes.Delete)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteDishAsync(Ulid id)
    {
        try
        {
            if (id == Ulid.Empty)
            {
                _logger.LogWarning($"Id не должен быть пустым.");
                return BadRequest(new { Error = "Id должен быть больше 0." });
            }

            await _dishAppService.DeleteDishAsync(id);
            _logger.LogInformation($"Блюдо с Id {id} успешно удалено.");
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при создании блюда.");
            return StatusCode(500, new { Error = "Произошла ошибка на сервере" });
        }
    }

    [HttpPut(ApiEndPoints.Dishes.Update)]
    public async Task<IActionResult> UpdateDishAsync(Ulid id, DishUpdateDto input)
    {
        if (id == Ulid.Empty)
        {
            _logger.LogWarning($"Id не должен быть пустым.");
            return BadRequest(new { Error = "Id должен быть больше 0." });
        }

        if (input is null)
        {
            _logger.LogWarning("DishUpdateDto равен null");
            return BadRequest(new { Error = "DishUpdateDto равен null" });
        }

        try
        {
            await _dishAppService.UpdateDishAsync(id, input);
            _logger.LogInformation($"Блюдо с Id = {id} успешно обновлено.");
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при обновлении блюда.");
            return StatusCode(500, new { Error = "Произошла ошибка на сервере." });
        }
    }
}