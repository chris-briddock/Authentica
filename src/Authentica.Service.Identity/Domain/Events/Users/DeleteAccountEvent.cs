namespace Domain.Events;

/// <summary>
/// Represents an event that occurs when a user account is deleted.
/// </summary>
public sealed class DeleteAccountEvent
{
    /// <summary>
    /// Gets or sets the email address associated with the account that is being deleted.
    /// </summary>
    public string Email { get; set; } = default!;
}