namespace Api.Responses;

/// <summary>
/// Represents a response to an event with details about the event.
/// </summary>
public sealed class ActivityResponse
{
    /// <summary>
    /// Gets or sets the unique identifier for the event sequence.
    /// </summary>
    public string SequenceId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the type of the activity.
    /// </summary>
    public string ActivityType { get; set; } = default!;

    /// <summary>
    /// Gets or sets the date and time when the event occurred.
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Gets or sets the data associated with the event.
    /// </summary>
    public string Data { get; set; } = default!;
}

