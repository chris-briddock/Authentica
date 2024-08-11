using Api.Requests;

namespace Domain.Events;

/// <summary>
/// Represents an event for resetting a user's password on behalf on them.
/// </summary>
public class ResetPasswordAdminEvent : EventBase<AdminPasswordResetRequest>
{
    /// <summary>
    /// Gets or sets the payload containing the admin password reset request details.
    /// </summary>
    public override AdminPasswordResetRequest Payload { get; set; } = default!;
}
