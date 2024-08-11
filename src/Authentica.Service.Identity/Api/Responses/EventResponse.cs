namespace Api.Responses;

/// <summary>
/// Represents a response to an event with details about the event.
/// </summary>
public class EventResponse
{
    /// <summary>
    /// Gets or sets the unique identifier for the event sequence.
    /// </summary>
    public string SequenceId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the type of the event.
    /// </summary>
    public string EventType { get; set; } = default!;

    /// <summary>
    /// Gets or sets the date and time when the event occurred.
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Gets or sets the data associated with the event.
    /// </summary>
    public string Data { get; set; } = default!;
}

