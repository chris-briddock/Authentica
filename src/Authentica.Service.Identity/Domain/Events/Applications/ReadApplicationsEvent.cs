using Api.Responses;

namespace Domain.Events;

/// <summary>
/// Represents an event that occurs when applications are read.
/// </summary>
public sealed class ReadApplicationsEvent : EventBase<List<ReadApplicationResponse>>
{
    /// <summary>
    /// Gets or sets the payload containing the response data for reading applications.
    /// </summary>
    public override List<ReadApplicationResponse> Payload { get; set; } = [];
}
