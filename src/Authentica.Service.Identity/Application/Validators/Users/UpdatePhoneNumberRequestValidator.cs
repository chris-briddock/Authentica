using Api.Requests;
using FluentValidation;

namespace Application.Validators;

/// <summary>
/// Validator for <see cref="UpdatePhoneNumberRequest"/>.
/// </summary>
public class UpdatePhoneNumberRequestValidator : AbstractValidator<UpdatePhoneNumberRequest>
{
    /// <summary>
    /// Initializes a new instance of <see cref="UpdatePhoneNumberRequestValidator"/>.
    /// </summary>
    public UpdatePhoneNumberRequestValidator()
    {
        RuleFor(request => request.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Phone number must be a valid international format.");

        RuleFor(request => request.Token)
            .NotEmpty().WithMessage("Token is required.")
            .Length(6, 50).WithMessage("Token must be between 6 and 50 characters long.");
    }
}