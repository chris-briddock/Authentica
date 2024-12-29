using Fido2NetLib;

namespace Domain.Requests;

/// <summary>
/// Represents a request for multi-factor authentication using a passkey assertion.
/// </summary>
public sealed record MultiFactorPasskeyAssertionRequest
{
    /// <summary>
    /// Gets or sets the email address associated with the request.
    /// </summary>
    public string Email { get; set; } = default!;

    /// <summary>
    /// Gets or sets the authenticator assertion raw response.
    /// </summary>
    public AuthenticatorAssertionRawResponse Response { get; set; } = default!;
}