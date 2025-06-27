using CateringService.Application.DataTransferObjects.Requests;
using FluentValidation;

namespace CateringService.Application.Validators.User;

public sealed class UserCreateRequestValidator : AbstractValidator<AddUserRequest>
{
    public UserCreateRequestValidator()
    {
        RuleFor(x => x.UserType)
            .NotEmpty().WithMessage("Тип пользователя обязателен.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Имя обязательно.")
            .MaximumLength(128).WithMessage("Имя не должно превышать 128 символов.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Фамилия обязательна.")
            .MaximumLength(128).WithMessage("Фамилия не должна превышать 128 символов.");

        RuleFor(x => x.MiddleName)
            .MaximumLength(128).WithMessage("Отчество не должно превышать 128 символов.")
            .When(x => !string.IsNullOrWhiteSpace(x.MiddleName));

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email обязателен.")
            .MaximumLength(100).WithMessage("Email не должен превышать 100 символов.")
            .EmailAddress().WithMessage("Неверный формат email.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Номер телефона обязателен.")
            .MaximumLength(20).WithMessage("Номер телефона не должен превышать 20 символов.");

        RuleFor(x => x.PasswordHash)
            .NotEmpty().WithMessage("Хэш пароля обязателен.");

        RuleFor(x => x.TenantId)
            .NotEqual(Ulid.Empty).WithMessage("TenantId обязателен.");

        RuleFor(x => x.Role)
            .IsInEnum()
            .WithMessage("Недопустимая роль.")
            .When(x => x.UserType == "Broker");

        RuleFor(x => x.Position)
            .MaximumLength(64).WithMessage("Должность не должна превышать 64 символов.")
            .When(x => x.UserType == "Supplier");

        RuleFor(x => x.CompanyId)
            .NotNull().WithMessage("CompanyId обязателен для Supplier и корпоративных Customer.")
            .When(x => x.UserType == "Supplier" || x.UserType == "Customer_Corporate");

        RuleFor(x => x.AddressId)
            .NotNull().WithMessage("AddressId обязателен для индивидуальных Customer.")
            .When(x => x.UserType == "Customer_Individual");

        RuleFor(x => x.TaxNumber)
            .NotNull().WithMessage("ИНН обязателен.")
            .WithMessage("ИНН должен быть числом от 10 до 12 цифр.");
    }
}