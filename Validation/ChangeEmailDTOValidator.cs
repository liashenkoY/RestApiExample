using FluentValidation;
using RstApiExample.DTO.Requests;

namespace RstApiExample.Validation;

public class ChangeEmailDTOValidator: AbstractValidator<ChangeEmailDTO>
{
    public ChangeEmailDTOValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Invalid email format.");
    }
}