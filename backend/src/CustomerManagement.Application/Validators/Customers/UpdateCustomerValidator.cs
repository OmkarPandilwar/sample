using CustomerManagement.Application.Commands.Customers;
using FluentValidation;

namespace CustomerManagement.Application.Validators.Customers;

public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.CustomerName)
            .NotEmpty().WithMessage("Customer name is required.")
            .MaximumLength(100).WithMessage("Customer name cannot exceed 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.");

        RuleFor(x => x.AccountValue)
            .GreaterThanOrEqualTo(0).WithMessage("Account value cannot be negative.");

        RuleFor(x => x.Classification).IsInEnum();
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.Segment).IsInEnum();
    }
}
