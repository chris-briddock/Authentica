using Api.Requests;

namespace Domain.Events;

/// <summary>
/// Represents an event that occurs during the registration of an admin.
/// </summary>
public class RegisterAdminEvent : EventBase<RegisterRequest>
{
    /// <summary>
    /// Gets or sets the payload for the register admin event.
    /// The payload contains the registration request details.
    /// </summary>
    public override RegisterRequest Payload { get; set; } = default!;
}
