namespace Application.Activities;

/// <summary>
/// Represents an activity that occurs when mfa recovery codes are requested.
/// </summary>
public class MultiFactorRecoveryCodesActivity
{
    /// <summary>
    /// Gets or sets the email address of the user requesting the mfa recovery codes.
    /// </summary>
    public string Email { get; set; } = default!;
}