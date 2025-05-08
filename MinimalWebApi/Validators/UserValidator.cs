using FluentValidation;
using MinimalWebApi.Models;

namespace MinimalWebApi.Validators;

public class UserValidator : AbstractValidator<LoginRequestBody>
{
    public UserValidator()
    {
        RuleFor(user => user.Username)
            .NotEmpty()
            .WithMessage("UserName is required");
        
        RuleFor(User => User.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters long");
            
    }
}