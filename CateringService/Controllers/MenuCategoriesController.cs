using CateringService.Application.Abstractions;
using CateringService.Application.DataTransferObjects.MenuCategory;
using Microsoft.AspNetCore.Mvc;

namespace CateringService.Controllers
{
    [ApiController]
    public class MenuCategoriesController : ControllerBase
    {
        private readonly IMenuCategoryAppService _menuCategoryAppService;
        private readonly ILogger<MenuCategoriesController> _logger;

        public MenuCategoriesController(IMenuCategoryAppService menuCategoryAppService, ILogger<MenuCategoriesController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(menuCategoryAppService));
            _menuCategoryAppService = menuCategoryAppService ?? throw new ArgumentNullException(nameof(menuCategoryAppService));
        }

        [HttpGet(ApiEndPoints.MenuCategories.Get)]
        public async Task<ActionResult<List<MenuCategoryDto>>> GetBySupplierIdAsync(Ulid supplierId)
        {
            var menuCategories = _menuCategoryAppService.GetBySupplierIdAsync(supplierId);

            return Ok(menuCategories);
        }
    }
}