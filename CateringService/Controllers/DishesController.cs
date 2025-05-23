using AutoMapper;
using CateringService.Application.DataTransferObjects.Dish;
using CateringService.Domain.Abstractions;
using CateringService.Domain.Entities.Approved;
using CateringService.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CateringService.Controllers;

[ApiController]
[TypeFilter<LoggingActionFilter>]
public class DishesController : ControllerBase
{
    private readonly IDishService _dishService;
    private readonly IMapper _mapper;
    private readonly ILogger<DishesController> _logger;
    public DishesController(IDishService dishAppService, IMapper mapper, ILogger<DishesController> logger)
    {
        _dishService = dishAppService ?? throw new ArgumentNullException(nameof(dishAppService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet(ApiEndPoints.Dishes.GetAll)]
    [ProducesResponseType(typeof(IEnumerable<DishDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DishDto>>> GetDishesAsync()
    {
        var dishes = await _dishService.GetAllAsync();
        if (dishes is null || !dishes.Any())
        {
            _logger.LogInformation("Список блюд пуст.");
            return Ok(Enumerable.Empty<DishDto>());
        }

        var dishesDto = _mapper.Map<IEnumerable<DishDto>>(dishes);
        _logger.LogInformation($"Запрос списка блюд выполнен успешно. Найдено {dishesDto.Count()} блюд.");
        return Ok(dishesDto);
    }

    [HttpGet(ApiEndPoints.Dishes.Get, Name = "GetDishById")]
    [ProducesResponseType(typeof(DishDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DishDto>> GetDishAsync(Ulid dishId)
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

    [HttpPost(ApiEndPoints.Dishes.Create)]
    [ProducesResponseType(typeof(DishDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DishDto>> CreateDishAsync([FromBody] DishCreateDto input)
    {
        if (input is null)
        {
            _logger.LogWarning("Входные данные не указаны. Операция создания блюда не может быть выполнена.");
            return BadRequest(new { Error = "Входные параметры отсутствуют. Пожалуйста, предоставьте данные для создания блюда." });
        }

        if (!_dishService.CheckSupplierExists(input.SupplierId))
        {
            _logger.LogWarning($"Поставщик с Id = {input.SupplierId} не найден.");
            return NotFound($"Поставщик c Id = {input.SupplierId} не найден.");
        }

        if (!_dishService.CheckMenuCategoryExists(input.MenuCategoryId))
        {
            _logger.LogWarning($"Категория меню с Id = {input.MenuCategoryId} не найдена.");
            return NotFound($"Категория меню с Id = {input.MenuCategoryId} не найдена.");
        }

        var dish = _mapper.Map<Dish>(input);
        var createdDish = await _dishService.AddAsync(dish);
        if (createdDish is null)
        {
            _logger.LogWarning("Блюдо не было создано.");
            return BadRequest("Блюдо не было создано.");
        }

        var dishDto = _mapper.Map<DishDto>(createdDish);
        _logger.LogInformation($"Блюдо {dishDto.Name} с Id = {dishDto.Id} создано в {dishDto.CreatedAt}");
        return CreatedAtRoute("GetDishById",
        new
        {
            dishId = dishDto.Id
        },
        dishDto);
    }

    [HttpDelete(ApiEndPoints.Dishes.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteDishAsync(Ulid dishId)
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

    [HttpPut(ApiEndPoints.Dishes.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        var updateRequest = _mapper.Map<Dish>(input);

        await _dishService.UpdateAsync(dishId, updateRequest);

        _logger.LogInformation($"Блюдо с Id = {dishId} успешно обновлено.");
        return NoContent();
    }

    [HttpPatch(ApiEndPoints.Dishes.Toggle)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ToggleDishState(Ulid dishId)
    {
        var dish = await _dishService.GetByIdAsync(dishId);
        if (dish is null)
        {
            _logger.LogWarning($"Блюдо с Id = {dishId} не найдено.");
            return NotFound();
        }

        var result = await _dishService.ToggleDishStateAsync(dishId);

        return NoContent();
    }
}