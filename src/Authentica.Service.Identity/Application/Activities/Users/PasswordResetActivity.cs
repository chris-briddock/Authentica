using Api.Requests;

namespace Application.Activities;

/// <summary>
/// Represents an activity that occurs when a password reset request is made.
/// </summary>
public sealed class PasswordResetActivity : ActivityBase<PasswordResetRequest>
{
    /// <summary>
    /// Gets or sets the payload containing the request data for the password reset activity.
    /// </summary>
    public override PasswordResetRequest Payload { get; set; } = default!;
}
