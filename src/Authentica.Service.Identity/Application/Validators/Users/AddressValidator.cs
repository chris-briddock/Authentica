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
            .MaximumLength(64).WithMessage("Address name must be at most 64 characters long.")
            .When(address => !string.IsNullOrEmpty(address.Name));

        RuleFor(address => address.Number)
            .MaximumLength(10).WithMessage("Address number must be at most 10 characters long.")
            .When(address => !string.IsNullOrEmpty(address.Number));

        RuleFor(address => address.Street)
            .MaximumLength(200).WithMessage("Address street must be at most 200 characters long.")
            .When(address => !string.IsNullOrEmpty(address.Street));

        RuleFor(address => address.City)
            .MaximumLength(100).WithMessage("Address city must be at most 100 characters long.")
            .When(address => !string.IsNullOrEmpty(address.City));

        RuleFor(address => address.State)
            .MaximumLength(100).WithMessage("Address state must be at most 100 characters long.")
            .When(address => !string.IsNullOrEmpty(address.State));

        RuleFor(address => address.Postcode)
            .MaximumLength(10).WithMessage("Address postcode must be at most 10 characters long.")
            .When(address => !string.IsNullOrEmpty(address.Postcode));

        RuleFor(address => address.Country)
            .MaximumLength(100).WithMessage("Address country must be at most 100 characters long.")
            .When(address => !string.IsNullOrEmpty(address.Country));
    }
}
