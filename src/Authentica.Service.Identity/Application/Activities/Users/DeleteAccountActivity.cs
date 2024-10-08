namespace Application.Activities;

/// <summary>
/// Represents an activity that occurs when a user account is deleted.
/// </summary>
public sealed class DeleteAccountActivity
{
    /// <summary>
    /// Gets or sets the email address associated with the account that is being deleted.
    /// </summary>
    public string Email { get; set; } = default!;
}