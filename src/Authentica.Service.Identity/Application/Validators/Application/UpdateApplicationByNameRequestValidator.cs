using Api.Requests;
using Application.Extensions;
using FluentValidation;

namespace Application.Validators;

/// <summary>
/// Validator for <see cref="UpdateApplicationByNameRequest"/>.
/// </summary>
public class UpdateApplicationByNameRequestValidator : AbstractValidator<UpdateApplicationByNameRequest>
{
    /// <summary>
    /// Initializes a new instance of <see cref="UpdateApplicationByNameRequestValidator"/>
    /// </summary>
    public UpdateApplicationByNameRequestValidator()
    {
        RuleFor(request => request.CurrentName)
            .NotEmpty().WithMessage("currentName is required.")
            .Length(1, 100).WithMessage("currentName must be between 1 and 100 characters long.");

        RuleFor(request => request.NewName)
            .Length(1, 100).WithMessage("newName must be between 1 and 100 characters long.")
            .When(request => !string.IsNullOrEmpty(request.NewName));

        RuleFor(request => request.NewCallbackUri)
            .Must(uri => string.IsNullOrEmpty(uri) || uri.BeAValidUri())
            .WithMessage("newCallbackUri must be a valid URI.");
    }
}
