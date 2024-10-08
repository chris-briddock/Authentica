using Api.Requests;

namespace Application.Activities;

/// <summary>
/// Represents an activity that occurs when the mfa authentication settings are managed.
/// </summary>
public class MultiFactorManageActivity : ActivityBase<MultiFactorManageRequest>
{
    /// <summary>
    /// Gets or sets the email address of the user whose mfa authentication settings are being managed.
    /// </summary>
    public string Email { get; set; } = default!;

    /// <summary>
    /// Gets or sets the payload containing the request data for managing mfa settings.
    /// </summary>
    public override MultiFactorManageRequest Payload { get; set; } = default!;
}
