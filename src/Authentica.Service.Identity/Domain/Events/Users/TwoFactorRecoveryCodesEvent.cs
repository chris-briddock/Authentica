namespace Domain.Events;

/// <summary>
/// Represents an event that occurs when two-factor authentication recovery codes are requested.
/// </summary>
public class TwoFactorRecoveryCodesEvent
{
    /// <summary>
    /// Gets or sets the email address of the user requesting the two-factor authentication recovery codes.
    /// </summary>
    public string Email { get; set; } = default!;
}