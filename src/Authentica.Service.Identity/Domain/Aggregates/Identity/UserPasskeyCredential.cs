namespace Domain.Aggregates.Identity;

/// <summary>
/// Represents a user-passkey crednetial link with a specific key type.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
public class UserPasskeyCredential<TKey> where TKey : IEquatable<TKey>
{
    public virtual TKey Id { get; set; } = default!;
    public virtual TKey UserId { get; set; } = default!;
    public virtual TKey PasskeyCredentialId { get; set; } = default!;

    public User User { get; set; } = default!;
    public PasskeyCredential PasskeyCredential { get; set; } = default!;
}

/// <summary>
/// Represents a default implementation for the user-passkey crednetial link with a string key.
/// </summary>
public sealed class UserPasskeyCredential : UserPasskeyCredential<string>
{
    /// <inheritdoc />
    public override string Id { get; set; } = Guid.NewGuid().ToString();
}
