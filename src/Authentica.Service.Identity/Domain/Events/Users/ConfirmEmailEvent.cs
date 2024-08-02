using Api.Requests;

namespace Domain.Events;

/// <summary>
/// Represents an event that occurs when an email confirmation request is made.
/// </summary>
public sealed class ConfirmEmailEvent : EventBase<ConfirmEmailRequest>
{
    /// <summary>
    /// Gets or sets the payload containing the request data for confirming an email.
    /// </summary>
    public override ConfirmEmailRequest Payload { get; set; } = default!;
}
