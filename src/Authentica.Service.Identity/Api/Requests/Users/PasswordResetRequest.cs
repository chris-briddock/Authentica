using Application.Attributes;

namespace Api.Requests;

/// <summary>
/// Represents a request to reset a user's password.
/// </summary>
public sealed record PasswordResetRequest
{
    /// <summary>
    /// Gets or sets the email address of the user requesting the password reset.
    /// </summary>
    public string Email { get; init; } = default!;

    /// <summary>
    /// Gets or sets the reset token provided to the user for password reset verification.
    /// </summary>
    [SensitiveData]
    public string Token { get; init; } = default!;

    /// <summary>
    /// Gets or sets the new password for the user.
    /// </summary>
    [SensitiveData]
    public required string Password { get; init; } = default!;
}
