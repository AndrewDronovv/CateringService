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
            if (dishes is null || !dishes.Any())
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
    public async Task<ActionResult<DishDto>> GetDishAsync(Ulid dishId)
    {
        try
        {
            _logger.LogInformation($"Получен запрос блюда с Id = {dishId}.");
            if (dishId == Ulid.Empty)
            {
                _logger.LogWarning($"Id не должен быть пустым.");
                return BadRequest(new { Error = "Id не должен быть пустым." });
            }

            var dish = await _dishService.GetByIdAsync(dishId);
            if (dish is null)
            {
                _logger.LogWarning($"Блюдо с Id = {dishId} не найдено.");
                return NotFound(new { Error = $"Блюдо с Id = {dishId} не найдено." });
            }

            var dishDto = _mapper.Map<DishDto>(dish);

            _logger.LogInformation($"Блюдо {dishDto.Name} с Id = {dishId} успешно получено.");
            return Ok(dishDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ошибка при получении блюда по Id = {dishId}.");
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

            if (!_dishService.CheckSupplierExists(input.SupplierId))
            {
                return NotFound($"Поставщик c Id = {input.SupplierId} не найден");
            }

            if (!_dishService.CheckMenuCategoryExists(input.MenuCategoryId))
            {
                return NotFound($"Категория меню с Id = {input.MenuCategoryId} не найдена");
            }

            var dish = _mapper.Map<Dish>(input);

            var createdDish = await _dishService.AddAsync(dish);
            if (createdDish is null)
            {
                return BadRequest($"Блюдо не было создано");
            }


            _logger.LogInformation($"Блюдо {createdDish} с Id = {createdDish.Id} создано в {createdDish.CreatedAt}");
            return CreatedAtRoute("GetDishById",
                new
                {
                    dishId = createdDish.Id
                },
                _mapper.Map<DishDto>(createdDish));

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при создании блюда.");
            return StatusCode(500, new { Error = "Произошла ошибка на сервере." });
        }
    }

    [HttpDelete(ApiEndPoints.Dishes.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteDishAsync(Ulid dishId)
    {
        try
        {
            if (dishId == Ulid.Empty)
            {
                _logger.LogWarning($"Id не должен быть пустым.");
                return BadRequest(new { Error = "Id не должен быть пустым." });
            }

            var deletedDish = await _dishService.GetByIdAsync(dishId);

            await _dishService.DeleteAsync(dishId);
            _logger.LogInformation($"Блюдо {deletedDish} с Id = {dishId} успешно удалено.");
            return Ok(new
            {
                Success = true,
                Message = $"Выбранное блюдо {deletedDish?.Name} удалено",
                Timestamp = DateTime.Now
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при удалении блюда.");
            return StatusCode(500, new { Error = "Произошла ошибка на сервере" });
        }
    }

    [HttpPut(ApiEndPoints.Dishes.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateDishAsync(Ulid dishId, DishUpdateDto input)
    {
        if (dishId == Ulid.Empty)
        {
            _logger.LogWarning($"Id не должен быть пустым.");
            return BadRequest(new { Error = "Id не должен быть пустым." });
        }

        if (input is null)
        {
            _logger.LogWarning("Входные данные не указаны. Операция обновления блюда не может быть выполнена.");
            return BadRequest(new { Error = "Входные параметры отсутствуют. Пожалуйста, предоставьте данные для создания блюда." });
        }

        try
        {
            var updateRequest = _mapper.Map<Dish>(input);

            await _dishService.UpdateAsync(dishId, updateRequest);

            _logger.LogInformation($"Блюдо с Id = {dishId} успешно обновлено.");
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при обновлении блюда.");
            return StatusCode(500, new { Error = "Произошла ошибка на сервере." });
        }
    }

    [HttpPatch(ApiEndPoints.Dishes.Toggle)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ToggleDishState(Ulid dishId)
    {
        try
        {
            var dish = await _dishService.GetByIdAsync(dishId);
            if (dish is null)
            {
                return NotFound();
            }

            var result = await _dishService.ToggleDishState(dishId);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при переключении состояния блюда.");
            return StatusCode(500, new { Error = "Произошла ошибка на сервере." });
        }
    }
}