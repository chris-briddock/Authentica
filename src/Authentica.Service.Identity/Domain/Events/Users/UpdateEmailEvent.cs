using Api.Requests;

namespace Domain.Events;

/// <summary>
/// Represents an event that occurs when a user's email address is updated.
/// </summary>
public class UpdateEmailEvent : EventBase<UpdateEmailRequest>
{
    /// <summary>
    /// Gets or sets the payload containing the request data for updating the user's email address.
    /// </summary>
    public override UpdateEmailRequest Payload { get; set; } = default!;
}
