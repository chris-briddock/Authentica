using Api.Requests;

namespace Domain.Events;

/// <summary>
/// Represents an event that occurs when a new application is created.
/// </summary>
public sealed class CreatedApplicationEvent : EventBase<CreateApplicationRequest>
{
    /// <summary>
    /// Gets or sets the payload containing the request data for creating an application.
    /// </summary>
    public override CreateApplicationRequest Payload { get; set; } = default!;
}
