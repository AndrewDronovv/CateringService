using CateringService.Application.DataTransferObjects.Dish;
using FluentValidation;

namespace CateringService.Application.Validators.Dish;

public class DishUpdateDtoValidator : AbstractValidator<DishUpdateDto>
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
            .GreaterThan(0).WithMessage("Вес должен быть больше 0.")
            .LessThanOrEqualTo(10000).WithMessage("Вес не может превышать 10.000 грамм.");

        RuleFor(x => x.IsAvailable)
            .NotNull().WithMessage("Поле доступности (IsAvailable) должно быть указано.");

        RuleFor(x => x.PortionSize)
            .MaximumLength(150).WithMessage("Размер порции не должен превышать 150 символов.");

        RuleFor(x => x.Allergens)
            .MaximumLength(400).WithMessage("Список аллергенов не должен превышать 400 символов.");

        RuleFor(x => x.CreatedAt)
            .NotEmpty().WithMessage("Дата создания не должна быть пустой.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Дата создания не может быть в будущем.");

        RuleFor(x => x.SupplierId)
            .Must(id => id != Ulid.Empty).WithMessage("Идентификатор поставщика не может быть пустым.")
            .Must(id => Ulid.TryParse(id.ToString(), out _)).WithMessage("Идентификатор поставщица должен быть корректным Ulid.");

        RuleFor(x => x.MenuCategoryId)
            .Must(id => id != Ulid.Empty).WithMessage("Идентификатор категории не может быть пустым.")
            .Must(id => Ulid.TryParse(id.ToString(), out _)).WithMessage("Идентификатор категории меню должен быть корректным Ulid.");
    }
}