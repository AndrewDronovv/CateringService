using CateringService.Application.DataTransferObjects.Responses;
using CateringService.Domain.Entities.Approved;

namespace CateringService.Domain.Abstractions;

public interface IMenuCategoryService
{
    Task<List<MenuCategoryViewModel>> GetMenuCategoriesAsync(Ulid supplierId);
    Task<List<MenuCategory>> GetCategoriesAsync(Ulid supplilerId);
    Task<MenuCategory> GetByIdAndSupplierIdAsync(Ulid categoryId, Ulid supplierId);
    Task DeleteCategoryAsync(Ulid categoryId, Ulid supplierId);
}