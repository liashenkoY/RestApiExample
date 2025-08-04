using FluentValidation;
using RstApiExample.DTO.Requests;

namespace RstApiExample.Validation;

public class UserLogInDTOValidator: AbstractValidator<UserLogInDTO>
{
    public UserLogInDTOValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Invalid email format.");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }

}