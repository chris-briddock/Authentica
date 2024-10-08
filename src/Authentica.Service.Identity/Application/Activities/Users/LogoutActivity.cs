namespace Application.Activities;

/// <summary>
/// Represents an activity that occurs when a user logs out.
/// </summary>
public class LogoutActivity
{
    /// <summary>
    /// Gets or sets the email address of the user who is logging out.
    /// </summary>
    public string Email { get; set; } = default!;
}
