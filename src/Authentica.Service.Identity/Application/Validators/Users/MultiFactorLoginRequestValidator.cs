using Api.Requests;
using FluentValidation;

namespace Application.Validators;

/// <summary>
/// Validator for <see cref="MultiFactorLoginRequest"/>.
/// </summary>
public class MultiFactorLoginRequestValidator : AbstractValidator<MultiFactorLoginRequest>
{
    /// <summary>
    /// Initializes a new instance of <see cref="MultiFactorLoginRequestValidator"/>.
    /// </summary>
    public MultiFactorLoginRequestValidator()
    {
        RuleFor(request => request.Token)
            .NotEmpty().WithMessage("MFA token is required.")
            .Length(6, 6).WithMessage("MFA token must be exactly 6 characters long.");
    }
}
