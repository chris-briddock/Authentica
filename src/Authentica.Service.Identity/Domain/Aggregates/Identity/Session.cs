namespace Domain.Aggregates.Identity;

/// <summary>
/// Represents a user session within the system.
/// </summary>
public sealed class Session
{
    /// <summary>
    /// Gets or sets the unique identifier for the session.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the session identifier.
    /// </summary>
    /// <remarks>
    /// This is used to uniquely identify a user's session within the system.
    /// It is different from the Id property, which is the entity's unique identifier.
    /// </remarks>
    public string SessionId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the user ID associated with this session.
    /// </summary>
    public string UserId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the start time of the session.
    /// </summary>
    public DateTime StartDateTime { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the end time of the session. Null if the session is still active.
    /// </summary>
    public DateTime? EndDateTime { get; set; } = default!;

    /// <summary>
    /// Gets or sets the IP address from which the session originated.
    /// </summary>
    public string IpAddress { get; set; } = default!;

    /// <summary>
    /// Gets or sets the user agent string of the client that initiated the session.
    /// </summary>
    public string UserAgent { get; set; } = default!;
    /// <summary>
    /// Gets or sets the current status of the session.
    /// </summary>
    public string Status { get; set; } = default!;
}
