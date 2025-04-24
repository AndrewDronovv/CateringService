using AutoMapper;
using CateringService.Application.DataTransferObjects.MenuCategory;
using CateringService.Domain.Abstractions;
using CateringService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CateringService.Controllers;

[ApiController]
public class MenuCategoriesController : ControllerBase
{
    private readonly IMenuCategoryService _menuCategoryService;
    private readonly ILogger<MenuCategoriesController> _logger;
    private readonly IMapper _mapper;

    public MenuCategoriesController(IMenuCategoryService menuCategoryService, ILogger<MenuCategoriesController> logger, IMapper mapper)
    {
        _menuCategoryService = menuCategoryService ?? throw new ArgumentNullException(nameof(menuCategoryService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet(ApiEndPoints.MenuCategories.GetAll)]
    [ProducesResponseType(typeof(List<MenuCategoryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<MenuCategoryDto>>> GetMenuCategoriesBySupplierIdAsync(Ulid supplierId)
    {
        _logger.LogInformation($"Получен запрос на категории меню у поставщика с Id = {supplierId}.");

        var menuCategories = await _menuCategoryService.GetCategoriesAsync(supplierId);
        if (menuCategories == null || menuCategories.Count == 0)
        {
            _logger.LogInformation($"Список категорий меню у поставщика с Id = {supplierId} пуст.");
            return Ok(Enumerable.Empty<MenuCategoryDto>());
        }

        _logger.LogInformation(@$"Запрос списка категорий меню выполнен успешно. У поставщика с Id = {supplierId} 
            найдено {menuCategories.Count} категорий меню.");

        var menuCategoriesDto = _mapper.Map<List<MenuCategoryDto>>(menuCategories);
        return Ok(menuCategoriesDto);
    }

    [HttpGet(ApiEndPoints.MenuCategories.Get)]
    [ProducesResponseType(typeof(MenuCategoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MenuCategoryDto>> GetMenuCategoryBySupplierId(Ulid categoryId, Ulid supplierId)
    {
        _logger.LogInformation($"Получен запрос на категорию меню с Id = {categoryId} и поставщка с Id = {supplierId}.");

        if (categoryId == Ulid.Empty || supplierId == Ulid.Empty)
        {
            _logger.LogWarning("Id категории или поставщика не должны быть пустыми.");
            return BadRequest(new { Error = "Id категории и поставщика обязательны." });
        }

        var menuCategory = await _menuCategoryService.GetByIdAndSupplierIdAsync(categoryId, supplierId);
        if (menuCategory == null)
        {
            _logger.LogInformation($"Категория меню с Id = {categoryId} и поставщика с Id = {supplierId} не существует.");
            return NotFound(new { Error = "Категория меню не найдена." });
        }

        _logger.LogInformation($"Запрос категории меню выполнен успешно.");
        var menuCategoryDto = _mapper.Map<MenuCategoryDto>(menuCategory);
        return Ok(menuCategoryDto);
    }

    [HttpPost(ApiEndPoints.MenuCategories.Create)]
    [ProducesResponseType(typeof(MenuCategoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MenuCategoryDto>> CreateMenuCategoryAsync([FromBody] MenuCategoryCreateDto input)
    {
        try
        {
            if (input is null)
            {
                _logger.LogWarning("Входные данные не указаны. Операция создания категории меню не может быть выполнена.");
                return BadRequest(new { Error = "Входные параметры отсутствуют. Пожалуйста, предоставьте данные для создания категории меню." });
            }

            var menuCategory = _mapper.Map<MenuCategory>(input);

            var createdMenuCategory = await _menuCategoryService.AddAsync(menuCategory);
            if (createdMenuCategory == null)
            {
                return BadRequest($"Категория меню не была создана");
            }

            _logger.LogInformation($"Категория меню {createdMenuCategory} с Id = {createdMenuCategory.Id} создана в {createdMenuCategory.CreatedAt}");
            return CreatedAtRoute("GetMenuCategoryById",
                new
                {
                    id = createdMenuCategory.Id
                },
                createdMenuCategory);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при создании блюда.");
            return StatusCode(500, new { Error = "Произошла ошибка на сервере." });
        }
    }
}