using FluentValidation;
using RstApiExample.DTO.Requests;

namespace RstApiExample.Validation;

public class UserCreateOrUpdateDTOValidator: AbstractValidator<UserCreateOrUpdateDTO>
{
    public UserCreateOrUpdateDTOValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters.");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters.");
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
    }
}