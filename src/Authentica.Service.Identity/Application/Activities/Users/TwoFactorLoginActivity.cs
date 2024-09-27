using Api.Requests;

namespace Application.Activities;

/// <summary>
/// Represents an activity that occurs when a two-factor authentication login request is made.
/// </summary>
public sealed class TwoFactorLoginActivity : ActivityBase<TwoFactorLoginRequest>
{
    /// <summary>
    /// Gets or sets the payload containing the request data for the two-factor login activity.
    /// </summary>
    public override TwoFactorLoginRequest Payload { get; set; } = default!;
}
