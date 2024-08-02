using Api.Requests;

namespace Domain.Events;

/// <summary>
/// Represents an event that occurs when an application is deleted.
/// </summary>
public sealed class DeleteApplicationEvent : EventBase<DeleteApplicationByNameRequest>
{
    /// <summary>
    /// Gets or sets the payload containing the request data for deleting an application by name.
    /// </summary>
    public override DeleteApplicationByNameRequest Payload { get; set; } = default!;
}
