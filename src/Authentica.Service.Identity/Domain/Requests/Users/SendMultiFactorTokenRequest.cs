namespace Domain.Requests;

/// <summary>
/// Represents a request to send a multi-factor authentication token.
/// </summary>
public sealed class SendMultiFactorTokenRequest
{
    /// <summary>
    /// The user's email address.
    /// </summary>
    public string Email { get; set; } = default!;
}