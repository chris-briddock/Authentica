using Api.Requests;
using FluentValidation;

namespace Application.Validators;

/// <summary>
/// Validator for <see cref="UpdateEmailRequest"/>.
/// </summary>
public class UpdateEmailRequestValidator : AbstractValidator<UpdateEmailRequest>
{
    /// <summary>
    /// Initializes a new instance of <see cref="UpdateEmailRequestValidator"/>.
    /// </summary>
    public UpdateEmailRequestValidator()
    {
        RuleFor(request => request.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be a valid email address.");

        RuleFor(request => request.Token)
            .NotEmpty().WithMessage("Token is required.")
            .Length(6, 50).WithMessage("Token must be between 6 and 50 characters long.");
    }
}
