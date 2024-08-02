using Domain.ValueObjects;
using FluentValidation;

namespace Application.Validators;

/// <summary>
/// Validator for <see cref="Address"/>.
/// </summary>
public class AddressValidator : AbstractValidator<Address>
{
    /// <summary>
    /// Initializes a new instance of <see cref="AddressValidator"/>.
    /// </summary>
    public AddressValidator()
    {
        RuleFor(address => address.Name)
            .NotEmpty().WithMessage("Address name is required.")
            .MaximumLength(64).WithMessage("Address name must be at most 64 characters long.");

        RuleFor(address => address.Number)
            .NotEmpty().WithMessage("Address number is required.")
            .MaximumLength(10).WithMessage("Address number must be at most 10 characters long.");

        RuleFor(address => address.Street)
            .NotEmpty().WithMessage("Address street is required.")
            .MaximumLength(200).WithMessage("Address street must be at most 200 characters long.");

        RuleFor(address => address.City)
            .NotEmpty().WithMessage("Address city is required.")
            .MaximumLength(100).WithMessage("Address city must be at most 100 characters long.");

        RuleFor(address => address.State)
            .NotEmpty().WithMessage("Address state is required.")
            .MaximumLength(100).WithMessage("Address state must be at most 100 characters long.");

        RuleFor(address => address.PostCode)
            .NotEmpty().WithMessage("Address postcode is required.")
            .MaximumLength(10).WithMessage("Address postcode must be at most 10 characters long.");

        RuleFor(address => address.Country)
            .NotEmpty().WithMessage("Address country is required.")
            .MaximumLength(100).WithMessage("Address country must be at most 100 characters long.");
    }
}
