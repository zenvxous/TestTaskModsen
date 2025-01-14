using FluentValidation;
using TestTaskModsen.Core.Models;

namespace TestTaskModsen.Application.Validators;

public class EventValidator : AbstractValidator<Event>
{
    public EventValidator()
    {
        RuleFor(e => e.Id)
            .NotEmpty().WithMessage("Id cannot be empty.");
        
        RuleFor(e => e.Title)
            .NotEmpty().WithMessage("Title cannot be empty.")
            .MaximumLength(100).WithMessage("Title cannot be longer than 100 characters.");
        
        RuleFor(e => e.Description)
            .MaximumLength(500).WithMessage("Description cannot be longer than 500 characters.");

        RuleFor(e => e.StartDate)
            .NotEmpty().WithMessage("Start date cannot be empty.");
        
        RuleFor(e => e.EndDate)
            .NotEmpty().WithMessage("End date cannot be empty.");
        
        RuleFor(e => e.Location)
            .NotEmpty().WithMessage("Location cannot be empty.")
            .MaximumLength(50).WithMessage("Location cannot be longer than 50 characters.");
        
        RuleFor(e => e.Category)
            .IsInEnum().WithMessage("Invalid event category.");

        RuleFor(e => e.Capacity)
            .NotEmpty().WithMessage("Capacity cannot be empty.");
    }
}