using FluentValidation;
using Shared.DTO;

namespace ProjectBackEnd.Validation;

public class UserLoginDTOValidator : AbstractValidator<UserLoginDTO>
{
    public UserLoginDTOValidator()
    {
        RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress().WithMessage("Email is not a valid email address");
    }
}