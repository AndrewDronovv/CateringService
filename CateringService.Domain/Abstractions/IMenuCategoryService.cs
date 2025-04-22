using CateringService.Domain.Entities;

namespace CateringService.Domain.Abstractions;

public interface IMenuCategoryService : IBaseService<MenuCategory, Ulid>
{
    Task<List<MenuCategory>> GetBySupplierIdAsync(Ulid supplilerId);
}
