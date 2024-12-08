using Api.Requests;
using FluentValidation;

namespace Application.Validators;

/// <summary>
/// Validator for <see cref="LoginRequest"/>.
/// </summary>
public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    /// <summary>
    /// Initializes a new instance of <see cref="LoginRequestValidator"/>.
    /// </summary>
    public LoginRequestValidator()
    {
        RuleFor(request => request.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be a valid email address.");

        RuleFor(request => request.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(12).WithMessage("Password must be at least 12 characters long.");
    }
}
