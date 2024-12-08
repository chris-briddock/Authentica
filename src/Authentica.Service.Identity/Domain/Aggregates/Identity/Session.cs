using Domain.Contracts;
using Domain.ValueObjects;

namespace Domain.Aggregates.Identity;

/// <summary>
/// Represents a user session within the system.
/// </summary>
public sealed class Session :
    IEntityDeletionStatus<string>,
    IEntityCreationStatus<string>
{
    /// <summary>
    /// Gets or sets the unique identifier for the session entity.
    /// </summary>
    /// <remarks>
    /// This is automatically generated as a new GUID string when the session is created.
    /// It serves as the primary key for the session entity in the database.
    /// </remarks>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the session identifier.
    /// </summary>
    /// <remarks>
    /// This is used to uniquely identify a user's session within the system.
    /// It is different from the Id property, which is the entity's unique identifier.
    /// This could be used for session management and tracking purposes.
    /// </remarks>
    public string SessionId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the user ID associated with this session.
    /// </summary>
    /// <remarks>
    /// This links the session to a specific user in the system.
    /// It can be used to retrieve user-specific information or apply user-specific settings during the session.
    /// </remarks>
    public string UserId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the start time of the session.
    /// </summary>
    /// <remarks>
    /// This is automatically set to the current UTC time when the session is created.
    /// It can be used to calculate session duration or for auditing purposes.
    /// </remarks>
    public DateTime StartDateTime { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the end time of the session.
    /// </summary>
    /// <remarks>
    /// This is null if the session is still active.
    /// When set, it indicates that the session has ended, and can be used to calculate the total session duration.
    /// </remarks>
    public DateTime? EndDateTime { get; set; } = default!;

    /// <summary>
    /// Gets or sets the IP address from which the originated.
    /// </summary>
    /// <remarks>
    /// This can be used for security auditing or geographical tracking purposes.
    /// It's nullable to account for scenarios where the IP address might not be available or applicable.
    /// </remarks>
    public string? IpAddress { get; set; } = default!;

    /// <summary>
    /// Gets or sets the user agent string of the client that initiated the session.
    /// </summary>
    /// <remarks>
    /// This typically contains information about the user's browser and operating system.
    /// It can be used for analytics, debugging, or tailoring the user experience.
    /// </remarks>
    public string UserAgent { get; set; } = default!;

    /// <summary>
    /// Gets or sets the current status of the session.
    /// </summary>
    /// <remarks>
    /// This could represent states such as "Active", "Expired", "Terminated", etc.
    /// It can be used to quickly determine the current state of the session without examining other properties.
    /// </remarks>
    public string Status { get; set; } = default!;

    /// <summary>
    /// Gets or sets the deletion status of the entity.
    /// </summary>
    /// <remarks>
    /// This property tracks whether the entity has been soft-deleted, 
    /// along with metadata about the deletion event (like the timestamp and user responsible).
    /// </remarks>
    public EntityDeletionStatus<string> EntityDeletionStatus { get; set; } = default!;

    /// <summary>
    /// Gets or sets the creation status of the entity.
    /// </summary>
    /// <remarks>
    /// This property contains information about the creation of the entity.
    /// It includes whether the creation was successful and any relevant messages.
    /// </remarks>
    public EntityCreationStatus<string> EntityCreationStatus { get; set; } = default!;
}