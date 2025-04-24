using CateringService.Application.DataTransferObjects.MenuCategory;
using FluentValidation;

namespace CateringService.Application.Validators.MenuCategory;

public class MenuCategoryDtoValidator : AbstractValidator<MenuCategoryCreateDto>
{
    public MenuCategoryDtoValidator()
    {
        
    }
}
