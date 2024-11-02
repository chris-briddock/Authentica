namespace Domain.Aggregates.Identity;

/// <summary>
/// Represents a registered passkey credential for a user, compliant with FIDO2 standards.
/// </summary>
public sealed class PasskeyCredential
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public byte[] CredentialId { get; set; } = default!;
    public byte[] PublicKey { get; set; } = default!;
    public byte[] UserHandle { get; set; } = default!;
    public uint SignatureCounter { get; set; }
    public string CredType { get; set; } = default!;
    public DateTime RegDate { get; set; }
    public Guid AaGuid { get; set; }

    // Associations with the User
    public string UserId { get; set; } = default!;
    public User User { get; set; } = default!;
}
