using CateringService.Domain.Entities;
using CateringService.Domain.Repositories;

namespace CateringService.Persistence.Repositories;

public class MenuCategoryRepository : BaseRepository<MenuCategory, Ulid>, IMenuCategoryRepository
{
    public MenuCategoryRepository(AppDbContext context) : base(context)
    {
    }

    public Task<List<MenuCategory>> GetBySupplierIdAsync(Ulid supplierId)
    {
        throw new NotImplementedException();
    }
}
