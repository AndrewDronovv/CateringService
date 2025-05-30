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

    public MenuCategoriesController(IMenuCategoryService menuCategoryService, ISupplierService supplierService)
    {
        _menuCategoryService = menuCategoryService ?? throw new ArgumentNullException(nameof(menuCategoryService));
        _supplierService = supplierService ?? throw new ArgumentNullException(nameof(supplierService)); ;
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
    public async Task<ActionResult<MenuCategoryViewModel>> GetMenuCategoryBySupplierIdAsync(Ulid menuCategoryId, Ulid supplierId)
    {
        var menuCategory = await _menuCategoryService.GetMenuCategoryBySupplierIdAsync(menuCategoryId, supplierId);

        return Ok(menuCategory);
    }

    [HttpPost(ApiEndPoints.MenuCategories.Create)]
    [ProducesResponseType(typeof(MenuCategoryViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MenuCategoryViewModel>> CreateMenuCategoryAsync([FromBody] AddMenuCategoryRequest request, Ulid supplierId)
    {
        var createdMenuCategory = await _menuCategoryService.CreateMenuCategoryAsync(request, supplierId);

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
            return BadRequest(new { Error = "Id категории меню или Id поставщика не должны быть пустыми." });
        }

        await _menuCategoryService.DeleteMenuCategoryAsync(categoryId, supplierId);
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