using Api.Requests;

namespace Domain.Events;

/// <summary>
/// Represents an event that occurs when a two-factor authentication login request is made.
/// </summary>
public sealed class TwoFactorLoginEvent : EventBase<TwoFactorLoginRequest>
{
    /// <summary>
    /// Gets or sets the payload containing the request data for the two-factor login event.
    /// </summary>
    public override TwoFactorLoginRequest Payload { get; set; } = default!;
}
