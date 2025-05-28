using AutoMapper;
using CateringService.Application.Abstractions;
using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.DataTransferObjects.Responses;
using CateringService.Domain.Abstractions;
using CateringService.Domain.Entities.Approved;
using CateringService.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CateringService.Controllers;

[ApiController]
[TypeFilter<LoggingActionFilter>]
public class MenuCategoriesController : ControllerBase
{
    private readonly IMenuCategoryService _menuCategoryService;
    private readonly ISupplierService _supplierService;
    private readonly ILogger<MenuCategoriesController> _logger;
    private readonly IMapper _mapper;

    public MenuCategoriesController(IMenuCategoryService menuCategoryService, ILogger<MenuCategoriesController> logger,
        IMapper mapper, ISupplierService supplierService)
    {
        _menuCategoryService = menuCategoryService ?? throw new ArgumentNullException(nameof(menuCategoryService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _supplierService = supplierService ?? throw new ArgumentNullException(nameof(supplierService)); ;
    }

    [HttpGet(ApiEndPoints.MenuCategories.GetAll)]
    [ProducesResponseType(typeof(List<MenuCategoryViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<MenuCategoryViewModel>>> GetMenuCategoriesAsync(Ulid supplierId)
    {
        _logger.LogInformation($"Получен запрос на категории меню у поставщика с Id = {supplierId}.");

        if (supplierId == Ulid.Empty)
        {
            _logger.LogWarning("Id поставщика не должен быть пустым.");
            return BadRequest(new { Error = "Id поставщика не должен быть пустым." });
        }

        var menuCategories = await _menuCategoryService.GetCategoriesAsync(supplierId);
        if (menuCategories is null || menuCategories.Count == 0)
        {
            _logger.LogInformation($"Список категорий меню у поставщика с Id = {supplierId} пуст или равен 0.");
            return Ok(Enumerable.Empty<MenuCategoryViewModel>());
        }

        _logger.LogInformation($"Запрос списка категорий меню выполнен успешно. У поставщика с Id = {supplierId} найдено {menuCategories.Count} категорий меню.");
        var menuCategoriesDto = _mapper.Map<List<MenuCategoryViewModel>>(menuCategories);
        return Ok(menuCategoriesDto);
    }

    [HttpGet(ApiEndPoints.MenuCategories.Get, Name = "GetMenuCategoryById")]
    [ProducesResponseType(typeof(MenuCategoryViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MenuCategoryViewModel>> GetMenuCategoryAsync(Ulid categoryId, Ulid supplierId)
    {
        _logger.LogInformation($"Получен запрос на категорию меню с Id = {categoryId} и поставщка с Id = {supplierId}.");

        if (categoryId == Ulid.Empty || supplierId == Ulid.Empty)
        {
            _logger.LogWarning("Id категории меню или поставщика не должны быть пустыми.");
            return BadRequest(new { Error = "Id категории меню и поставщика обязательны." });
        }

        var menuCategory = await _menuCategoryService.GetByIdAndSupplierIdAsync(categoryId, supplierId);
        if (menuCategory is null)
        {
            _logger.LogInformation($"Категория меню с Id = {categoryId} и поставщика с Id = {supplierId} не существует.");
            return NotFound(new { Error = "Категория меню не найдена." });
        }

        _logger.LogInformation($"Запрос категории меню выполнен успешно.");
        var menuCategoryDto = _mapper.Map<MenuCategoryViewModel>(menuCategory);
        return Ok(menuCategoryDto);
    }

    //[HttpPost(ApiEndPoints.MenuCategories.Create)]
    //[ProducesResponseType(typeof(MenuCategoryViewModel), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    //public async Task<ActionResult<MenuCategoryViewModel>> CreateMenuCategoryAsync([FromBody] AddMenuCategoryRequest input, Ulid supplierId)
    //{
    //    if (input is null)
    //    {
    //        _logger.LogWarning("Входные данные не указаны. Операция создания категории меню не может быть выполнена.");
    //        return BadRequest(new { Error = "Входные параметры отсутствуют. Пожалуйста, предоставьте данные для создания категории меню." });
    //    }

    //    if (supplierId == Ulid.Empty)
    //    {
    //        _logger.LogWarning("Id не должен быть пустым.");
    //        return BadRequest(new { Error = "Id не должен быть пустым." });
    //    }

    //    var supplierData = await _supplierService.GetByIdAsync(supplierId);
    //    if (supplierData is null)
    //    {
    //        _logger.LogInformation($"Поставщика с Id = {supplierId} не существует.");
    //        return NotFound(new { Error = "Поставщик не найден." });
    //    }

    //    var menuCategory = _mapper.Map<MenuCategory>(input);
    //    var createdMenuCategory = await _menuCategoryService.AddAsync(menuCategory);
    //    if (createdMenuCategory == null)
    //    {
    //        return BadRequest($"Категория меню не была создана.");
    //    }

    //    var menuCategoryDto = _mapper.Map<MenuCategoryViewModel>(menuCategory);
    //    _logger.LogInformation($"Категория меню {menuCategoryDto.Name} с Id = {menuCategoryDto.Id} создана в {menuCategoryDto.CreatedAt}");
    //    return CreatedAtRoute("GetMenuCategoryById",
    //        new
    //        {
    //            categoryId = menuCategoryDto.Id,
    //            supplierId = supplierId
    //        },
    //            menuCategoryDto
    //        );
    //}

    [HttpDelete(ApiEndPoints.MenuCategories.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteMenuCategoryAsync(Ulid categoryId, Ulid supplierId)
    {
        if (categoryId == Ulid.Empty || supplierId == Ulid.Empty)
        {
            _logger.LogWarning($"Id категории меню или Id поставщика не должны быть пустыми.");
            return BadRequest(new { Error = "Id категории меню или Id поставщика не должны быть пустыми." });
        }

        await _menuCategoryService.DeleteCategoryAsync(categoryId, supplierId);
        _logger.LogInformation($"Категория меню с Id = {categoryId} у поставщика с Id = {supplierId} удалена.");
        return NoContent();
    }

    //[HttpPut(ApiEndPoints.MenuCategories.Update)]
    //[ProducesResponseType(StatusCodes.Status204NoContent)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //public async Task<IActionResult> UpdateMenuCategoryAsync(Ulid supplierId, Ulid categoryId, UpdateMenuCategoryRequest input)
    //{
    //    if (categoryId == Ulid.Empty)
    //    {
    //        _logger.LogWarning($"Id не должен быть пустым.");
    //        return BadRequest(new { Error = "Id не должен быть пустым." });
    //    }

    //    if (input is null)
    //    {
    //        _logger.LogWarning("Входные данные не указаны. Операция обновления категории меню не может быть выполнена.");
    //        return BadRequest(new { Error = "Входные параметры отсутствуют. Пожалуйста, предоставьте данные для создания категории меню." });
    //    }

    //    var updateRequest = _mapper.Map<MenuCategory>(input);

    //    await _menuCategoryService.UpdateAsync(categoryId, updateRequest);

    //    _logger.LogInformation($"Категория меню с Id = {categoryId} успешно обновлена.");
    //    return NoContent();
    //}
}