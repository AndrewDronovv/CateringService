using CateringService.Domain.Entities;

namespace CateringService.Domain.Abstractions;

public interface IMenuCategoryService : IBaseService<MenuCategory, Ulid>
{
    Task<List<MenuCategory>> GetCategoriesAsync(Ulid supplilerId);
    Task<MenuCategory> GetByIdAndSupplierIdAsync(Ulid categoryId, Ulid supplierId);
    Task DeleteCategoryAsync(Ulid categoryId, Ulid supplierId);
}