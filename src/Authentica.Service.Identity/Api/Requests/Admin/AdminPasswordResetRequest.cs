namespace Api.Requests;

/// <summary>
/// Represents a request to reset an admin password.
/// </summary>
public class AdminPasswordResetRequest
{
    /// <summary>
    /// Gets or sets the email address associated with the admin account.
    /// </summary>
    public string Email { get; set; } = default!;
    /// <summary>
    /// Gets or sets the new password. 
    /// </summary>
    public string Password { get; set; } = default!;
}
