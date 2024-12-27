namespace Domain.Requests;

/// <summary>
/// Represents a request for a multi-factor passkey login.
/// </summary>
public sealed record MultiFactorPasskeyLoginRequest
{
    /// <summary>
    /// Gets or sets the email address of the user attempting to log in.
    /// </summary>
    public required string Email { get; set; } = default!;
}
