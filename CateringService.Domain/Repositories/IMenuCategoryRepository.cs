using CateringService.Domain.Entities.Approved;

namespace CateringService.Domain.Repositories;

public interface IMenuCategoryRepository : IGenericRepository<MenuCategory, Ulid>
{
    Task<List<MenuCategory>> GetBySupplierIdAsync(Ulid supplierId);
    Task<MenuCategory> GetByIdAndSupplierIdAsync(Ulid supplierId, Ulid menuCategoryId);
    Task DeleteAsync(Ulid categoryId, Ulid supplierId);
    Task<bool> HasDishesAsync(Ulid categoryId);
}