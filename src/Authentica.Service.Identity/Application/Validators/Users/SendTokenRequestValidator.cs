using Api.Requests;
using Authentica.Common;
using FluentValidation;

namespace Application.Validators;

/// <summary>
/// Validates the <see cref="SendTokenRequest"/> to 
/// ensure that the provided email and token type meet the required criteria.
/// </summary>
public class SendTokenRequestValidator : AbstractValidator<SendTokenRequest>
{
    /// <summary>
    /// List of allowed token types for validation.
    /// </summary>
    private static readonly string[] AllowedTokenTypes = 
    { 
        EmailTokenConstants.ResetPassword,
        EmailTokenConstants.MultiFactor,
        EmailTokenConstants.ConfirmEmail,
        EmailTokenConstants.UpdateEmail,
        EmailTokenConstants.UpdatePhoneNumber
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="SendTokenRequestValidator"/> class and defines validation rules.
    /// </summary>
    public SendTokenRequestValidator()
    {
        RuleFor(request => request.Email)
            .NotEmpty().WithMessage("Email address is required.")
            .EmailAddress().WithMessage("Invalid email address format.");

        RuleFor(request => request.TokenType)
            .NotEmpty().WithMessage("Token type is required.")
            .Must(BeValidTokenType).WithMessage($"Invalid token type. Allowed values are: {string.Join(", ", AllowedTokenTypes)}.");
    }

    /// <summary>
    /// Validates the allowed values for token type.
    /// </summary>
    /// <param name="tokenType">The token type to validate.</param>
    /// <returns>
    /// <c>true</c> if the token type is valid; otherwise, <c>false</c>.
    /// </returns>
    private bool BeValidTokenType(string tokenType)
    {
        return AllowedTokenTypes.Contains(tokenType);
    }
}

