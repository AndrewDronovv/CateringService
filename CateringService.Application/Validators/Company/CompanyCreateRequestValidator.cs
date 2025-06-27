using CateringService.Application.DataTransferObjects.Requests;
using FluentValidation;

namespace CateringService.Application.Validators.Company;

public sealed class AddCompanyRequestValidator : AbstractValidator<AddCompanyRequest>
{
    public AddCompanyRequestValidator()
    {
        RuleFor(x => x.TenantId)
            .NotEqual(Ulid.Empty).WithMessage("Идентификатор арендатора не может быть пустым.")
            .Must(id => Ulid.TryParse(id.ToString(), out _)).WithMessage("Идентификатор арендатора должен быть корректным Ulid.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название компании не должно быть пустым.")
            .MinimumLength(2).WithMessage("Название компании должно содержать не менее 2 символов.")
            .MaximumLength(200).WithMessage("Название компании не должно превышать 200 символов.");

        RuleFor(x => x.TaxNumber)
            .NotEmpty().WithMessage("Налоговый номер компании не должен быть пустым.")
            .MinimumLength(10).WithMessage("Налоговый номер компании должен содержать не менее 10 символов.")
            .MaximumLength(20).WithMessage("Налоговый номер компании не должен превышать 20 символов.");

        RuleFor(x => x.AddressId)
            .NotEqual(Ulid.Empty).WithMessage("Идентификатор адреса не может быть пустым.");

        RuleFor(x => x.Phone)
            .MaximumLength(20).WithMessage("Номер телефона не должен превышать 20 символов.")
            .When(x => !string.IsNullOrWhiteSpace(x.Phone));

        RuleFor(x => x.Email)
            .MaximumLength(100).WithMessage("Длина почты не должна превышать 100 символов.")
            .EmailAddress().WithMessage("Некорректный формат электронной почты.")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));
    }
}