using CateringService.Application.DataTransferObjects.Dish;
using FluentValidation;

namespace CateringService.Application.Validators.Dish;

public class DishCreateDtoValidator : AbstractValidator<DishCreateDto>
{
    public DishCreateDtoValidator()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Название не должно быть пустым.")
            .MinimumLength(4).WithMessage("Название должно содержать не менее 4 символов.")
            .MaximumLength(100).WithMessage("Название не должно превышать 100 символов.")
            .Matches(@"^[a-zA-Zа-яА-Я\s]+$").WithMessage("Название должно содержать только буквы и пробелы.");

        RuleFor(x => x.Descritpion)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Описание не должно быть пустым.")
            .MinimumLength(10).WithMessage("Описание должно содержать не менее 10 символов.")
            .MaximumLength(500).WithMessage("Описание не должно превышать 500 символов.");

        RuleFor(x => x.Price)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Цена не должна быть пустой.")
            .GreaterThan(0).WithMessage("Цена должна быть больше 0.")
            .LessThanOrEqualTo(10000).WithMessage("Цена не может превышать 10.000.");

        RuleFor(x => x.Ingredients)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Ингредиенты не должны быть пустыми.")
            .MaximumLength(1000).WithMessage("Ингредиенты не должны превышать 1000 символов.");

        RuleFor(x => x.Weight)
            .GreaterThan(0).WithMessage("Вес должен быть больше 0.");

        RuleFor(x => x.SupplierId)
            .GreaterThan(0).WithMessage("Идентификатор поставщика должен быть больше 0.");

        RuleFor(x => x.MenuSectionId)
            .GreaterThan(0).WithMessage("Идентификатор секции меню должен быть больше 0.");
    }
}