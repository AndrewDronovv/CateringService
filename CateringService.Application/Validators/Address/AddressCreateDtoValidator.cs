using CateringService.Application.DataTransferObjects.Requests;
using FluentValidation;

namespace CateringService.Application.Validators.Address;

public sealed class AddressCreateDtoValidator : AbstractValidator<AddAddressRequest>
{
    public AddressCreateDtoValidator()
    {
        RuleFor(x => x.TenantId)
            .Must(id => id != Ulid.Empty).WithMessage("Идентификатор арендатора не может быть пустым.")
            .Must(id => Ulid.TryParse(id.ToString(), out _)).WithMessage("Идентификатор арендатора должен быть корректным Ulid.");

        RuleFor(x => x.Country)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Название страны не должно быть пустым.")
            .MinimumLength(4).WithMessage("Название страны должно содержать не менее 4 символов.")
            .MaximumLength(64).WithMessage("Название страны не должно превышать 64 символа.");

        RuleFor(x => x.StreetAndBuilding)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Название улицы и здания не должны быть пустыми.")
            .MinimumLength(4).WithMessage("Название улицы и здания должны содержать не менее 4 символов.")
            .MaximumLength(128).WithMessage("Название улицы и здания не должны превышать 128 символа.");

        RuleFor(x => x.Zip)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Zip код обязателен.")
            .Matches(@"^\d+$").WithMessage("Zip код должен содержать только цифры.")
            .Length(6).WithMessage("Zip код должен содержать ровно 6 цифр.");

        RuleFor(x => x.City)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Название города не должно быть пустым.")
            .MinimumLength(4).WithMessage("Название города должно содержать не менее 4 символов.")
            .MaximumLength(64).WithMessage("Название города не должно превышать 64 символа.");

        RuleFor(x => x.Region)
            .MaximumLength(64).WithMessage("Название региона не должно превышать 64 символа.");

        RuleFor(x => x.Comment)
            .MaximumLength(256).WithMessage("Комментарий не должен превышать 256 символов.");

        RuleFor(x => x.Description)
            .MaximumLength(512).WithMessage("Описание не должно превышать 512 символа.");
    }
}