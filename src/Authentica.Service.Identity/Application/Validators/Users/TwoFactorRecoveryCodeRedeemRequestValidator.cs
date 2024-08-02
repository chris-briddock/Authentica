using Api.Requests;
using FluentValidation;

namespace Application.Validators
{
    /// <summary>
    /// Validator for <see cref="TwoFactorRecoveryCodeRedeemRequest"/>.
    /// </summary>
    public class TwoFactorRecoveryCodeRedeemRequestValidator : AbstractValidator<TwoFactorRecoveryCodeRedeemRequest>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="TwoFactorRecoveryCodeRedeemRequestValidator"/>.
        /// </summary>
        public TwoFactorRecoveryCodeRedeemRequestValidator()
        {
            RuleFor(request => request.Email)
                .NotEmpty().WithMessage("Email address is required.")
                .EmailAddress().WithMessage("Invalid email address format.");

            RuleFor(request => request.Code)
                .NotEmpty().WithMessage("Recovery code is required.")
                .Length(6, 10).WithMessage("Recovery code must be between 6 and 10 characters long."); // Adjust length as needed
        }
    }
}