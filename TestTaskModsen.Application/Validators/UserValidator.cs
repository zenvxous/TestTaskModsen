using FluentValidation;
using TestTaskModsen.Core.Models;

namespace TestTaskModsen.Application.Validators;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(user => user.Id)
            .NotEmpty().WithMessage("Id cannot be empty.");

        RuleFor(user => user.FirstName)
            .NotEmpty().WithMessage("First name cannot be empty.")
            .Length(2, 100).WithMessage("First name must be between 2 and 100 characters.")
            .Matches(@"^[A-Z][a-zA-Z\-]*$").WithMessage("First name must contain only letters.")
            .Must(name => !name.Contains("--")).WithMessage("First name mustn't contain more than 2 hyphens in a row.");

        RuleFor(user => user.LastName)
            .NotEmpty().WithMessage("Last name cannot be empty.")
            .Length(2, 100).WithMessage("Last name must be between 2 and 100 characters.")
            .Matches(@"^[A-Z][a-zA-Z\-]*$").WithMessage("Last name must contain only letters.")
            .Must(name => !name.Contains("--")).WithMessage("Last name mustn't contain more than 2 hyphens in a row.");

        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Email cannot be empty.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(user => user.PasswordHash)
            .NotEmpty().WithMessage("Password cannot be empty.");
        
        RuleFor(user => user.Role)
            .IsInEnum().WithMessage("Invalid user role.");
    }
}