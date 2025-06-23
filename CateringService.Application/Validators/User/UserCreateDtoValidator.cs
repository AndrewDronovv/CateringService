using CateringService.Application.DataTransferObjects.Requests;
using FluentValidation;

namespace CateringService.Application.Validators.User;

public sealed class UserCreateDtoValidator : AbstractValidator<AddUserRequest>
{
    public UserCreateDtoValidator()
    {
        
    }
}
