using Api.Requests;

namespace Domain.Events;

/// <summary>
/// Represents an event that occurs when a user registration request is made.
/// </summary>
public sealed class RegisterEvent : EventBase<RegisterRequest>
{
    /// <summary>
    /// Gets or sets the payload containing the request data for the user registration event.
    /// </summary>
    public override RegisterRequest Payload { get; set; } = default!;
}
