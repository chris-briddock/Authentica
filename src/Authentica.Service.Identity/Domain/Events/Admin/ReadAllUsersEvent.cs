namespace Domain.Events;

/// <summary>
/// Represents an event for reading all users with specific details.
/// </summary>
public class ReadAllUsersEvent
{
    /// <summary>
    /// Gets or sets the email address associated with the user.
    /// </summary>
    /// <remarks>
    /// This property holds the email address of the user which is used to filter or identify users in the event.
    /// </remarks>
    public string Email { get; set; } = default!;
}
