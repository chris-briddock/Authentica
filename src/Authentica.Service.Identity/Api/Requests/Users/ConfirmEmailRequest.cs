using Application.Attributes;

namespace Api.Requests;

/// <summary>
/// Represents an email confirmation.
/// </summary>
public sealed record ConfirmEmailRequest
{
    /// <summary>
    /// The users email address
    /// </summary>
    public required string Email { get; set; } = default!;
    /// <summary>
    /// The code to confirm the email address
    /// </summary>
    [SensitiveData]
    public required string Token { get; set; } = default!;

}
