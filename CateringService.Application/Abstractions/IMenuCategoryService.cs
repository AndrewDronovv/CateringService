using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.DataTransferObjects.Responses;

namespace CateringService.Domain.Abstractions;

public interface IMenuCategoryService
{
    Task<List<MenuCategoryViewModel>> GetMenuCategoriesAsync(Ulid supplilerId);
    Task<MenuCategoryViewModel> GetByIdAndSupplierIdAsync(Ulid categoryId, Ulid supplierId);
    Task<MenuCategoryViewModel> CreateMenuCategoryAsync(AddMenuCategoryRequest request, Ulid supplierId);
    Task DeleteCategoryAsync(Ulid categoryId, Ulid supplierId);
    Task<MenuCategoryViewModel> UpdateMenuCategoryAsync(Ulid menuCategoryId, Ulid supplierId, UpdateMenuCategoryRequest request);
}