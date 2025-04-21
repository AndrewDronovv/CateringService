using CateringService.Domain.Entities;

namespace CateringService.Domain.Repositories;

public interface IMenuCategoryRepository : IBaseRepository<MenuCategory, Ulid>
{
    Task<List<MenuCategory>> GetBySupplierIdAsync(Ulid supplierId);
}