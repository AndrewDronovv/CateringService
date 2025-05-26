using CateringService.Application.DataTransferObjects.Requests;
using FluentValidation;

namespace CateringService.Application.Validators.Dish;

public sealed class DishUpdateDtoValidator : AbstractValidator<UpdateDishRequest>
{
    public DishUpdateDtoValidator()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Название не должно быть пустым.")
            .MinimumLength(4).WithMessage("Название должно содержать не менее 4 символов.")
            .MaximumLength(100).WithMessage("Название не должно превышать 100 символов.")
            .Matches(@"^[a-zA-Zа-яА-Я\s]+$").WithMessage("Название должно содержать только буквы и пробелы.");

        RuleFor(x => x.Description)
            .Cascade(CascadeMode.Stop)
            .MinimumLength(10).WithMessage("Описание должно содержать не менее 10 символов.")
            .MaximumLength(500).WithMessage("Описание не должно превышать 500 символов.");

        RuleFor(x => x.Price)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Цена не должна быть пустой.")
            .GreaterThan(0).WithMessage("Цена должна быть больше 0.")
            .LessThanOrEqualTo(10000).WithMessage("Цена не может превышать 10.000.");
    }
}