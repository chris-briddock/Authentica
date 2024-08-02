using Api.Requests;
using Application.Extensions;
using FluentValidation;

namespace Application.Validators;

/// <summary>
/// Validator for <see cref="CreateApplicationRequest"/>.
/// </summary>
public class CreateApplicationRequestValidator : AbstractValidator<CreateApplicationRequest>
{
    /// <summary>
    /// Initializes a new instance of <see cref="CreateApplicationRequestValidator"/>
    /// </summary>
    public CreateApplicationRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(1, 100).WithMessage("Name must be between 1 and 100 characters long.");

        RuleFor(request => request.CallbackUri)
            .NotEmpty().WithMessage("CallbackUri is required.")
            .Must(uri => uri.BeAValidUri()).WithMessage("CallbackUri must be a valid URI.");

        RuleFor(request => request.RedirectUri)
            .NotEmpty().WithMessage("RedirectUri is required.")
            .Must(uri => uri.BeAValidUri()).WithMessage("RedirectUri must be a valid URI.");
    }
}
