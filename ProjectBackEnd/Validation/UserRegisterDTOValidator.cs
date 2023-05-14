using FluentValidation;
using Shared.DTO;

namespace ProjectBackEnd.Validation;

public class UserRegisterDTOValidator : AbstractValidator<UserRegisterDTO>
{
    public UserRegisterDTOValidator()
    {
        RuleFor(x => x.FirstName).NotNull().NotEmpty().Length(1, 20).WithMessage("FirstName cannot be null and with size between 1 to 20");
        RuleFor(x => x.LastName).NotNull().NotEmpty().Length(1, 20).WithMessage("LastName cannot be null and with size between 1 to 20");
        RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress().WithMessage("Email is not a valid email address");
        RuleFor(x => x.PhoneNumber).NotNull().NotEmpty().WithMessage("PhoneNumber cannot be null");
    }
}