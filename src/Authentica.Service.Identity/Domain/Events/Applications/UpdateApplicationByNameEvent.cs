using Api.Requests;

namespace Domain.Events;

/// <summary>
/// Represents an event that occurs when an application is updated by name.
/// </summary>
public sealed class UpdateApplicationByNameEvent : EventBase<UpdateApplicationByNameRequest>
{
    /// <summary>
    /// Gets or sets the payload containing the request data for updating an application by name.
    /// </summary>
    public override UpdateApplicationByNameRequest Payload { get; set; } = default!;
}