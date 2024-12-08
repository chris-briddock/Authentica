namespace Domain.Aggregates.Identity;


/// <summary>
/// Represents a default implementation for the passkey credential for a user.
/// </summary>
public sealed class PasskeyCredential : PasskeyCredential<string>
{
    /// <inheritdoc/>
    public override string Id { get; set; } = Guid.NewGuid().ToString();
}

/// <summary>
/// Represents a registered passkey credential for a user, compliant with FIDO2 standards.
/// </summary>
/// <typeparam name="TKey">The primary key type.</typeparam>
public abstract class PasskeyCredential<TKey> where TKey : class
{
    /// <summary>
    /// A unique identifier for the credential.
    /// </summary>
    public virtual TKey Id { get; set; } = default!;

    /// <summary>
    /// The unique identifier for the credential, assigned by the authenticator.
    /// </summary>
    public byte[] CredentialId { get; set; } = default!;

    /// <summary>
    /// The public key associated with this credential, used for verifying signatures.
    /// </summary>
    public byte[] PublicKey { get; set; } = default!;

    /// <summary>
    /// A unique handle to identify the user associated with the credential.
    /// </summary>
    public byte[] UserHandle { get; set; } = default!;

    /// <summary>
    /// A counter to track the number of times the credential has been used for authentication.
    /// This helps prevent replay attacks.
    /// </summary>
    public uint SignatureCounter { get; set; }

    /// <summary>
    /// The type of credential, typically "public-key" for FIDO2 credentials.
    /// </summary>
    public string CredType { get; set; } = default!;

    /// <summary>
    /// The date and time when the credential was registered.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// The globally unique identifier (AAGUID) of the authenticator that created this credential.
    /// </summary>
    public string AaGuid { get; set; }

    /// <summary>
    /// The user associated with this credential.
    /// </summary>
    public ICollection<UserPasskeyCredential> UserPasskeyCredential { get; set; } = default!;
}

