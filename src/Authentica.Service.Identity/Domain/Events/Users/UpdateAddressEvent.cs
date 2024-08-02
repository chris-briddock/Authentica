using Api.Requests;

namespace Domain.Events;

/// <summary>
/// Represents an event that occurs when a user's address is updated.
/// </summary>
public class UpdateAddressEvent : EventBase<UpdateAddressRequest>
{
    /// <summary>
    /// Gets or sets the payload containing the request data for updating a user's address.
    /// </summary>
    public override UpdateAddressRequest Payload { get; set; } = default!;
}
