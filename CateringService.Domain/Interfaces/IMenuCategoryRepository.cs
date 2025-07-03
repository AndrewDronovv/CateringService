using CateringService.Domain.Entities;

namespace CateringService.Domain.Repositories;

public interface IMenuCategoryRepository : IGenericRepository<MenuCategory, Ulid>
{
    Task<bool> ChechMenuCategoryExists(Ulid menuCategoryId);
    Task<List<MenuCategory>> GetBySupplierIdAsync(Ulid supplierId);
    Task<MenuCategory?> GetMenuCategoryBySupplierIdAsync(Ulid menuCategoryId, Ulid supplierId);
    Task DeleteAsync(Ulid menuCategoryId, Ulid supplierId);
    Task<bool> HasDishesAsync(Ulid menuCategoryId);
}