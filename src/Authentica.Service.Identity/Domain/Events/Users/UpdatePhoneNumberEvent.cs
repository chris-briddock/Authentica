using Api.Requests;

namespace Domain.Events;

/// <summary>
/// Represents an event that occurs when a user's phone number is updated.
/// </summary>
public class UpdatePhoneNumberEvent : EventBase<UpdatePhoneNumberRequest>
{
    /// <summary>
    /// Gets or sets the payload containing the request data for updating the user's phone number.
    /// </summary>
    public override UpdatePhoneNumberRequest Payload { get; set; } = default!;
}
