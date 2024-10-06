namespace Api.Requests;

/// <summary>
/// Represents a request to disable mfa for a user.
/// </summary>
public class DisableMultiFactorRequest
{
    /// <summary>
    /// Gets or sets the email address of the user which mfa will be disabled.
    /// </summary>
    public string Email { get; set; } = default!;
}
