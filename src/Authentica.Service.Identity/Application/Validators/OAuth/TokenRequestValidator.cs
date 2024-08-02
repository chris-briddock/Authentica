using Application.Extensions;
using Api.Requests;
using FluentValidation;

namespace Application.Validators;

/// <summary>
/// Validator for <see cref="TokenRequest"/>.
/// </summary>
public class TokenRequestValidator : AbstractValidator<TokenRequest>
{
    /// <summary>
    /// Initializes a new instance of <see cref="TokenRequestValidator"/>.
    /// </summary>
    public TokenRequestValidator()
    {
        RuleFor(request => request.GrantType)
            .NotEmpty().WithMessage("grant_type is required.")
            .Must(grantType => grantType.BeAValidGrantType()).WithMessage("invalid grant_type.");

        RuleFor(request => request.ClientId)
            .NotEmpty().WithMessage("client_id is required.")
            .Must(clientId => clientId.BeAValidGuid()).WithMessage("client_id must be a valid GUID.");

        RuleFor(request => request.ClientSecret)
            .NotEmpty().WithMessage("client_secret is required.");

        RuleFor(request => request.RedirectUri)
            .NotEmpty().WithMessage("redirect_uri is required.")
            .Must(uri => uri.BeAValidUri()).WithMessage("redirect_uri must be a valid URI.");

        RuleFor(request => request.State)
            .NotEmpty().WithMessage("state is required.");

        // Optional fields validations
        RuleFor(request => request.Code)
            .NotEmpty().When(request => request.GrantType == "code").WithMessage("code is required when grant_type is 'code'.");

        RuleFor(request => request.RefreshToken)
            .NotEmpty().When(request => request.GrantType == "refresh_token").WithMessage("refresh_token is required when grant_type  is 'refresh_token'.");

        RuleFor(request => request.DeviceCode)
            .NotEmpty().When(request => request.GrantType == "device_code").WithMessage("device_code is required when grant_type is 'device_code'.");
    }
}
