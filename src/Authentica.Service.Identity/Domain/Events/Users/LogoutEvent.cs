namespace Domain.Events;

/// <summary>
/// Represents an event that occurs when a user logs out.
/// </summary>
public class LogoutEvent
{
    /// <summary>
    /// Gets or sets the email address of the user who is logging out.
    /// </summary>
    public string Email { get; set; } = default!;
}
