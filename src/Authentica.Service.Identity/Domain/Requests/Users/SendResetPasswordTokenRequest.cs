namespace Domain.Requests;

/// <summary>
/// Represents a request to send a password reset token.
/// </summary>
public sealed record SendResetPasswordTokenRequest
{
    /// <summary>
    /// The user's email address.
    /// </summary>
    public string Email { get; set; } = default!;
}