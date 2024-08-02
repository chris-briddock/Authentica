using Api.Requests;
using FluentValidation;

namespace Application.Validators;

/// <summary>
/// Validator for <see cref="UpdateAddressRequest"/>.
/// </summary>
public class UpdateAddressRequestValidator : AbstractValidator<UpdateAddressRequest>
{

    /// <summary>
    /// Initializes a new instance of <see cref="UpdateAddressRequestValidator"/>.
    /// </summary>
    public UpdateAddressRequestValidator()
    {

        RuleFor(request => request.Address)
        .NotNull().WithMessage("Address is required.")
        .SetValidator(new AddressValidator());
    }
}
