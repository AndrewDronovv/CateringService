using CateringService.Application.DataTransferObjects.MenuCategory;
using FluentValidation;

namespace CateringService.Application.Validators.MenuCategory;

public sealed class MenuCategoryUpdateDtoValidatior : AbstractValidator<MenuCategoryUpdateDto>
{
    public MenuCategoryUpdateDtoValidatior()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Название не должно быть пустым.")
            .MinimumLength(4).WithMessage("Название должно содержать не менее 4 символов.")
            .MaximumLength(100).WithMessage("Название не должно превышать 100 символов.")
            .Matches(@"^[a-zA-Zа-яА-Я\s'""\-]+$").WithMessage("Название должно содержать только буквы, пробелы, апострофы, кавычки и дефисы.");

        RuleFor(x => x.Description)
            .Cascade(CascadeMode.Stop)
            .MinimumLength(10).WithMessage("Описание должно содержать не менее 10 символов.")
            .MaximumLength(500).WithMessage("Описание не должно превышать 500 символов.");
    }
}