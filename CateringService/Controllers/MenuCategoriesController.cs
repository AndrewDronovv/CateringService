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

    public MenuCategoriesController(IMenuCategoryService menuCategoryService)
    {
        _menuCategoryService = menuCategoryService ?? throw new ArgumentNullException(nameof(menuCategoryService));
    }

    [HttpGet(ApiEndPoints.MenuCategories.GetAll)]
    [ProducesResponseType(typeof(List<MenuCategoryViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
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
    [ProducesResponseType(typeof(MenuCategoryViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MenuCategoryViewModel>> CreateMenuCategoryAsync([FromBody] AddMenuCategoryRequest request, Ulid supplierId)
    {
        var createdMenuCategory = await _menuCategoryService.CreateMenuCategoryAsync(supplierId, request);

        return CreatedAtRoute("GetMenuCategoryById", new { menuCategoryId = createdMenuCategory.Id, supplierId = supplierId }, createdMenuCategory);
    }

    [HttpDelete(ApiEndPoints.MenuCategories.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMenuCategoryAsync(Ulid menuCategoryId, Ulid supplierId)
    {
        await _menuCategoryService.DeleteMenuCategoryAsync(menuCategoryId, supplierId);

        return NoContent();
    }

    [HttpPut(ApiEndPoints.MenuCategories.Update)]
    [ProducesResponseType(typeof(MenuCategoryViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateMenuCategoryAsync(Ulid supplierId, Ulid menuCategoryId, UpdateMenuCategoryRequest request)
    {
        var viewModel = await _menuCategoryService.UpdateMenuCategoryAsync(menuCategoryId, supplierId, request);

        return Ok(viewModel);
    }
}