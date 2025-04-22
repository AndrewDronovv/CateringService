using CateringService.Application.DataTransferObjects.MenuCategory;

namespace CateringService.Application.Abstractions;

public interface IMenuCategoryAppService
{
    Task<List<MenuCategoryDto>> GetBySupplierIdAsync(Ulid supplierId);
}
