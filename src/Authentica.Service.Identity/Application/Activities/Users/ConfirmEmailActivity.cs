using Api.Requests;

namespace Application.Activities;

/// <summary>
/// Represents an activity that occurs when an email confirmation request is made.
/// </summary>
public sealed class ConfirmEmailActivity : ActivityBase<ConfirmEmailRequest>
{
    /// <summary>
    /// Gets or sets the payload containing the request data for confirming an email.
    /// </summary>
    public override ConfirmEmailRequest Payload { get; set; } = default!;
}
