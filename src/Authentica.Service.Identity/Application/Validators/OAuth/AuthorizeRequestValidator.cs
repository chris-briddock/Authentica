using Api.Requests;
using Application.Extensions;
using FluentValidation;

namespace Application.Validators;

/// <summary>
/// Validator for <see cref="AuthorizeRequest"/>.
/// </summary>
public class AuthorizeRequestValidator : AbstractValidator<AuthorizeRequest>
{
    /// <summary>
    /// Initializes a new instance of <see cref="AuthorizeRequestValidator"/>
    /// </summary>
    public AuthorizeRequestValidator()
    {
        RuleFor(request => request.ClientId)
            .NotEmpty().WithMessage("client_id is required.")
            .Must(clientId => clientId.BeAValidGuid()).WithMessage("client_id must be a valid GUID.");

        RuleFor(request => request.CallbackUri)
            .NotEmpty().WithMessage("callback_uri is required.")
            .Must(uri => uri.BeAValidUri()).WithMessage("callback_uri must be a valid URI.");

    }
}
