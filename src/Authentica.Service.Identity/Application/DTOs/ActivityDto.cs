namespace Application.DTOs;

/// <summary>
/// Represents a data transfer object for an Activity.
/// </summary>
public sealed class ActivityDto
{
    /// <summary>
    /// Gets or sets the session ID associated with the activity.
    /// </summary>
    public string SequenceId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the type of the activity.
    /// </summary>
    public string ActivityType { get; set; } = default!;

    /// <summary>
    /// Gets or sets the date and time when the activity was created.
    /// </summary>
    public DateTime CreatedOn { get; set; } = default!;

    /// <summary>
    /// Gets or sets the request associated with the activity.
    /// </summary>
    public string? Data { get; set; } = default!;
}
