using Api.Requests;
using FluentValidation;

namespace Application.Validators;

/// <summary>
/// Validator for <see cref="PasswordResetRequest"/>.
/// </summary>
public class PasswordResetRequestValidator : AbstractValidator<PasswordResetRequest>
{
    /// <summary>
    /// Initializes a new instance of <see cref="PasswordResetRequestValidator"/>.
    /// </summary>
    public PasswordResetRequestValidator()
    {
        RuleFor(request => request.Email)
            .NotEmpty().WithMessage("Email address is required.")
            .EmailAddress().WithMessage("Invalid email address format.");

        RuleFor(request => request.Token)
            .NotEmpty().WithMessage("Reset token is required.");

        RuleFor(request => request.NewPassword)
            .NotEmpty().WithMessage("New password is required.")
            .MinimumLength(12).WithMessage("New password must be at least 12 characters long.")
            .Matches(@"[A-Z]").WithMessage("New password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("New password must contain at least one lowercase letter.")
            .Matches(@"[0-9]").WithMessage("New password must contain at least one number.")
            .Matches(@"[\W_]").WithMessage("New password must contain at least one special character.");
    }
}
