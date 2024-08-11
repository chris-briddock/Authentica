namespace Api.Requests;

/// <summary>
/// Represents a request to disable two-factor authentication for a user.
/// </summary>
public class DisableTwoFactorRequest
{
    /// <summary>
    /// Gets or sets the email address of the user for whom two-factor authentication will be disabled.
    /// </summary>
    public string Email { get; set; } = default!;
}
