namespace Domain.Events;

/// <summary>
/// Represents an event to read all events associated with a specific email.
/// </summary>
public class ReadAllEventsEvent
{
    /// <summary>
    /// Gets or sets the email address associated with the events to be read.
    /// </summary>
    public string Email { get; set; } = default!;
}
