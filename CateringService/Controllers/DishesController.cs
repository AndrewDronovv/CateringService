using AutoMapper;
using CateringService.Application.DataTransferObjects.Dish;
using CateringService.Domain.Abstractions;
using CateringService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CateringService.Controllers;

[ApiController]
public class DishesController : ControllerBase
{
    private readonly IDishService _dishService;
    private readonly ILogger<DishesController> _logger;
    private readonly IMapper _mapper;

    public DishesController(IDishService dishAppService, ILogger<DishesController> logger, IMapper mapper)
    {
        _dishService = dishAppService ?? throw new ArgumentNullException(nameof(dishAppService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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
            var dishes = await _dishService.GetAllAsync();
            if (dishes == null || !dishes.Any())
            {
                _logger.LogInformation("Список блюд пуст.");
                return Ok(Enumerable.Empty<DishDto>());
            }

            var dishesDto = _mapper.Map<IEnumerable<DishDto>>(dishes);
            _logger.LogInformation($"Запрос списка блюд выполнен успешно. Найдено {dishes.Count()} блюд.");
            return Ok(dishesDto);
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
                _logger.LogWarning($"Id не должен быть пустым.");
                return BadRequest(new { Error = "Id не должен быть пустым." });
            }

            var dish = await _dishService.GetByIdAsync(id);
            if (dish == null)
            {
                _logger.LogWarning($"Блюдо с Id = {id} не найдено.");
                return NotFound(new { Error = $"Блюдо с Id = {id} не найдено." });
            }

            var dishDto = _mapper.Map<DishDto>(dish);

            _logger.LogInformation($"Блюдо {dishDto.Name} с Id = {id} успешно получено.");
            return Ok(dishDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ошибка при получении блюда по Id = {id}.");
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
                _logger.LogWarning("Входные данные не указаны. Операция создания блюда не может быть выполнена.");
                return BadRequest(new { Error = "Входные параметры отсутствуют. Пожалуйста, предоставьте данные для создания блюда." });
            }

            var dish = _mapper.Map<Dish>(input);

            var createdDish = await _dishService.AddAsync(dish);
            if (createdDish == null)
            {
                return BadRequest($"Блюдо не было создано");
            }

            _logger.LogInformation($"Блюдо {createdDish} с Id = {createdDish.Id} создано в {createdDish.CreatedAt}");
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
            return StatusCode(500, new { Error = "Произошла ошибка на сервере." });
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
                return BadRequest(new { Error = "Id не должен быть пустым." });
            }

            var deletedDish = await _dishService.GetByIdAsync(id);

            await _dishService.DeleteAsync(id);
            _logger.LogInformation($"Блюдо {deletedDish} с Id = {id} успешно удалено.");
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при создании блюда.");
            return StatusCode(500, new { Error = "Произошла ошибка на сервере" });
        }
    }

    [HttpPut(ApiEndPoints.Dishes.Update)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
            var updateRequest = _mapper.Map<Dish>(input);

            await _dishService.UpdateAsync(id, updateRequest);

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