using Api.Requests;

namespace Domain.Events;

/// <summary>
/// Represents an event that occurs when the two-factor authentication settings are managed.
/// </summary>
public class TwoFactorManageEvent : EventBase<TwoFactorManageRequest>
{
    /// <summary>
    /// Gets or sets the email address of the user whose two-factor authentication settings are being managed.
    /// </summary>
    public string Email { get; set; } = default!;

    /// <summary>
    /// Gets or sets the payload containing the request data for managing two-factor authentication settings.
    /// </summary>
    public override TwoFactorManageRequest Payload { get; set; } = default!;
}
