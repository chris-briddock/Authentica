using Api.Requests;

namespace Application.Activities;

/// <summary>
/// Represents an activity that occurs when a mfa login request is made.
/// </summary>
public sealed class MultiFactorLoginActivity : ActivityBase<MultiFactorLoginRequest>
{
    /// <summary>
    /// Gets or sets the payload containing the request data for the mfa login activity.
    /// </summary>
    public override MultiFactorLoginRequest Payload { get; set; } = default!;
}
