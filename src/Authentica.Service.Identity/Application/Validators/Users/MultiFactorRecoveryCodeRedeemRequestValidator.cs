using Api.Requests;
using FluentValidation;

namespace Application.Validators
{
    /// <summary>
    /// Validator for <see cref="MultiFactorRecoveryCodeRedeemRequest"/>.
    /// </summary>
    public class MultiFactorRecoveryCodeRedeemRequestValidator : AbstractValidator<MultiFactorRecoveryCodeRedeemRequest>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="MultiFactorRecoveryCodeRedeemRequestValidator"/>.
        /// </summary>
        public MultiFactorRecoveryCodeRedeemRequestValidator()
        {
            RuleFor(request => request.Email)
                .NotEmpty().WithMessage("Email address is required.")
                .EmailAddress().WithMessage("Invalid email address format.");

            RuleFor(request => request.Code)
                .NotEmpty().WithMessage("Recovery code is required.")
                .Length(6, 11).WithMessage("Recovery code must be between 6 and 11 characters long."); // Adjust length as needed
        }
    }
}