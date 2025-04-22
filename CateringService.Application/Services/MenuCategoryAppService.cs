using AutoMapper;
using CateringService.Application.Abstractions;
using CateringService.Application.DataTransferObjects.MenuCategory;
using CateringService.Domain.Abstractions;

namespace CateringService.Application.Services;

public class MenuCategoryAppService : IMenuCategoryAppService
{
    private readonly IMenuCategoryService _menuCategoryService;
    private readonly IMapper _mapper;

    public MenuCategoryAppService(IMenuCategoryService menuCategoryService, IMapper mapper)
    {
        _menuCategoryService = menuCategoryService;
        _mapper = mapper;
    }

    public async Task<List<MenuCategoryDto>> GetBySupplierIdAsync(Ulid supplierId)
    {
        var menuCategories = await _menuCategoryService.GetBySupplierIdAsync(supplierId);

        if (menuCategories is null)
            throw new ArgumentNullException(nameof(menuCategories));

        return _mapper.Map<List<MenuCategoryDto>>(menuCategories);
    }
}