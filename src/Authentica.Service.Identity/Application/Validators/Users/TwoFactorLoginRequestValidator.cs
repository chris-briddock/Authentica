using Api.Requests;
using FluentValidation;

namespace Application.Validators;

/// <summary>
/// Validator for <see cref="TwoFactorLoginRequest"/>.
/// </summary>
public class TwoFactorLoginRequestValidator : AbstractValidator<TwoFactorLoginRequest>
{
    /// <summary>
    /// Initializes a new instance of <see cref="TwoFactorLoginRequestValidator"/>.
    /// </summary>
    public TwoFactorLoginRequestValidator()
    {
        RuleFor(request => request.Token)
            .NotEmpty().WithMessage("Two-factor token is required.")
            .Length(6, 6).WithMessage("Two-factor token must be exactly 6 characters long."); 
    }
}
