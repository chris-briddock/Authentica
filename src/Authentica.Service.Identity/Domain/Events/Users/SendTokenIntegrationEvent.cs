using Api.Requests;

namespace Domain.Events;

/// <summary>
/// Represents an event that occurs when a user requests a token.
/// </summary>
public sealed class SendTokenIntegrationEvent : EventBase<SendTokenRequest>
{
    /// <summary>
    /// Gets or sets the payload of the event, which contains the details of the token request.
    /// </summary>
    public override SendTokenRequest Payload { get; set; } = default!;
}