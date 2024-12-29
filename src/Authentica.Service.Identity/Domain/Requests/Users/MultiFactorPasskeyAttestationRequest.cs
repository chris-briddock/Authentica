using Fido2NetLib;

namespace Domain.Requests;

/// <summary>
/// Represents a request for passkey-based multi-factor attestation.
/// </summary>
public sealed record MultiFactorPasskeyAttestationRequest
{
    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    public string Email { get; set; } = default!;
    /// <summary>
    /// Gets or sets the Attestation raw response.
    /// </summary>
    public AuthenticatorAttestationRawResponse Response { get; set; } = default!;
}
