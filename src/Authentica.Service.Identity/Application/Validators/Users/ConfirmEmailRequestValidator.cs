using Api.Requests;
using FluentValidation;

namespace Application.Validators;

/// <summary>
/// Validator for <see cref="ConfirmEmailRequest"/>.
/// </summary>
public class ConfirmEmailRequestValidator : AbstractValidator<ConfirmEmailRequest>
{
    /// <summary>
    /// Initializes a new instance of <see cref="ConfirmEmailRequestValidator"/>.
    /// </summary>
    public ConfirmEmailRequestValidator()
    {
        RuleFor(request => request.Email)
            .NotEmpty().WithMessage("Email address is required.")
            .EmailAddress().WithMessage("Invalid email address format.");

        RuleFor(request => request.Token)
            .NotEmpty().WithMessage("Token is required.")
            .Length(6, 256).WithMessage("Token must be between 6 and 256 characters long."); // Adjust length as per your token requirements
    }
}