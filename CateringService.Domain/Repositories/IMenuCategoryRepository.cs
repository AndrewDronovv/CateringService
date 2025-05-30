using CateringService.Domain.Entities.Approved;

namespace CateringService.Domain.Repositories;

public interface IMenuCategoryRepository : IGenericRepository<MenuCategory, Ulid>
{
    Task<List<MenuCategory>> GetBySupplierIdAsync(Ulid supplierId);
    Task<MenuCategory> GetMenuCategoryBySupplierIdAsync(Ulid menuCategoryId, Ulid supplierId);
    Task DeleteAsync(Ulid menuCategoryId, Ulid supplierId);
    Task<bool> HasDishesAsync(Ulid menuCategoryId);
}