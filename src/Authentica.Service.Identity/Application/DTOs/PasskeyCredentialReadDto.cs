namespace Application.DTOs;

/// <summary>
/// Data transfer object for reading passkey credentials.
/// </summary>
public sealed class PasskeyCredentialReadDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the passkey credential.
    /// </summary>
    public byte[] PasskeyCredentialId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the signature counter of the passkey credential.
    /// </summary>
    public uint SignatureCounter { get; set; } = 0;

    /// <summary>
    /// Gets or sets the user handle associated with the passkey credential.
    /// </summary>
    public byte[] UserHandle { get; set; } = default!;

    /// <summary>
    /// Gets or sets the public key of the passkey credential.
    /// </summary>
    public byte[] PublicKey { get; set; } = default!;    

}
