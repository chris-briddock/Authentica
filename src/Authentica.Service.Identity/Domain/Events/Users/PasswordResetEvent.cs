using Api.Requests;

namespace Domain.Events;

/// <summary>
/// Represents an event that occurs when a password reset request is made.
/// </summary>
public sealed class PasswordResetEvent : EventBase<PasswordResetRequest>
{
    /// <summary>
    /// Gets or sets the payload containing the request data for the password reset event.
    /// </summary>
    public override PasswordResetRequest Payload { get; set; } = default!;
}
