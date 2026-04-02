using CustomerManagement.Application.Commands.Interactions;
using FluentValidation;

namespace CustomerManagement.Application.Validators.Interactions;

public class LogInteractionValidator : AbstractValidator<LogInteractionCommand>
{
    public LogInteractionValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.Subject).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Details).NotEmpty();
        RuleFor(x => x.CreatedBy).NotEmpty();
    }
}
