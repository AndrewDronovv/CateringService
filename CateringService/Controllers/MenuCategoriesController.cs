using CateringService.Application.Abstractions;
using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.DataTransferObjects.Responses;
using CateringService.Domain.Abstractions;
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

    public MenuCategoriesController(IMenuCategoryService menuCategoryService, ILogger<MenuCategoriesController> logger,
         ISupplierService supplierService)
    {
        _menuCategoryService = menuCategoryService ?? throw new ArgumentNullException(nameof(menuCategoryService));
        _supplierService = supplierService ?? throw new ArgumentNullException(nameof(supplierService)); ;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet(ApiEndPoints.MenuCategories.GetAll)]
    [ProducesResponseType(typeof(List<MenuCategoryViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<MenuCategoryViewModel>>> GetMenuCategoriesAsync(Ulid supplierId)
    {
        var menuCategories = await _menuCategoryService.GetMenuCategoriesAsync(supplierId);

        return Ok(menuCategories);
    }

    [HttpGet(ApiEndPoints.MenuCategories.Get, Name = "GetMenuCategoryById")]
    [ProducesResponseType(typeof(MenuCategoryViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MenuCategoryViewModel>> GetMenuCategoryBySupplierIdAsync(Ulid supplierId, Ulid menuCategoryId)
    {
        if (supplierId == Ulid.Empty)
        {
            _logger.LogWarning("SupplierId не должен быть пустым.");
            return BadRequest("SupplierId is required.");
        }

        if (menuCategoryId == Ulid.Empty)
        {
            _logger.LogWarning("MenuCategoryId не должен быть пустым.");
            return BadRequest("MenuCategoryId is required.");
        }

        var menuCategory = await _menuCategoryService.GetByIdAndSupplierIdAsync(supplierId, menuCategoryId);
        if (menuCategory is null)
        {
            _logger.LogWarning("Категория меню {MenuCategoryId} у поставщика {SupplierId} не найдена.", menuCategory, supplierId);
            return NotFound(new { Message = "Категория меню {MenuCategoryId} у поставщика {SupplierId} не найдена.", menuCategory, supplierId });
        }

        return Ok(menuCategory);
    }

    [HttpPost(ApiEndPoints.MenuCategories.Create)]
    [ProducesResponseType(typeof(MenuCategoryViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MenuCategoryViewModel>> CreateMenuCategoryAsync([FromBody] AddMenuCategoryRequest request, Ulid supplierId)
    {
        var createdMenuCategory = await _menuCategoryService.CreateMenuCategoryAsync(request, supplierId);
        if (createdMenuCategory == null)
        {
            return BadRequest($"Категория меню не была создана.");
        }

        return CreatedAtRoute("GetMenuCategoryById",
            new
            {
                menuCategoryId = createdMenuCategory.Id,
                supplierId = supplierId
            },
                createdMenuCategory
            );
    }

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

    [HttpPut(ApiEndPoints.MenuCategories.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateMenuCategoryAsync(Ulid supplierId, Ulid menuCategoryId, UpdateMenuCategoryRequest request)
    {
        var viewModel = await _menuCategoryService.UpdateMenuCategoryAsync(menuCategoryId, supplierId, request);

        return Ok(viewModel);
    }
}