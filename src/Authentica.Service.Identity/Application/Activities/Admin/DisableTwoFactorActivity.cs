using Api.Requests;

namespace Application.Activities;

/// <summary>
/// Represents an activity that occurs when two-factor authentication is disabled.
/// </summary>
public sealed class DisableTwoFactorActivity : ActivityBase<DisableTwoFactorRequest>
{
    /// <summary>
    /// Gets or sets the payload containing the request.
    /// </summary>
    public override DisableTwoFactorRequest Payload { get; set; } = default!;
}
