using FluentValidation;
using Shared.DTO;

namespace ProjectBackEnd.Validation;

public class AddUserDTOValidator : AbstractValidator<AddUserDTO>
{
    public AddUserDTOValidator()
    {
        RuleFor(x => x.FirstName).NotNull().NotEmpty().Length(1, 20).WithMessage("Emri nuk mund te jete null dhe duhet te kete 1 deri ne 20 karaktere");
        RuleFor(x => x.LastName).NotNull().NotEmpty().Length(1, 20).WithMessage("Mbiemri nuk mund te jete null dhe duhet te kete 1 deri ne 20 karaktere");
        RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress().WithMessage("Adresa e emailit nuk ka format te sakte");
        RuleFor(x => x.PhoneNumber).NotNull().NotEmpty().WithMessage("Numri i telefonit nu mund te jete null");
    }
}