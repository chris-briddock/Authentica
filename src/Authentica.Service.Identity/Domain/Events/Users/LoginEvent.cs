using Api.Requests;

namespace Domain.Events;

/// <summary>
/// Represents an event that occurs when a login request is made.
/// </summary>
public class LoginEvent : EventBase<LoginRequest>
{
    /// <summary>
    /// Gets or sets the payload containing the request data for the login event.
    /// </summary>
    
    public override LoginRequest Payload { get; set; } = default!;
}
