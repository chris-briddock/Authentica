using Api.Requests;
using FluentValidation;

namespace Application.Validators;

/// <summary>
/// Validator for <see cref="ReadApplicationByNameRequestValidator"/>.
/// </summary>
public class ReadApplicationByNameRequestValidator : AbstractValidator<ReadApplicationByNameRequest>
{
    /// <summary>
    /// Initializes a new instance of <see cref="ReadApplicationByNameRequestValidator"/>
    /// </summary>
    public ReadApplicationByNameRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(1, 100).WithMessage("Name must be between 1 and 100 characters long.");
    }
}
