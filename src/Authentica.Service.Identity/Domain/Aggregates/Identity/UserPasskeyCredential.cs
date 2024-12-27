namespace Domain.Aggregates.Identity;

/// <summary>
/// Represents a link between a user and a passkey credential, with a specified key type.
/// </summary>
/// <typeparam name="TKey">The type of the key, which must implement <see cref="IEquatable{TKey}"/>.</typeparam>
public class UserPasskeyCredential<TKey> where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Gets or sets the unique identifier for the user-passkey credential link.
    /// </summary>
    public virtual TKey Id { get; set; } = default!;

    /// <summary>
    /// Gets or sets the unique identifier of the user associated with this credential link.
    /// </summary>
    public virtual TKey UserId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the unique identifier of the passkey credential associated with this link.
    /// </summary>
    public virtual TKey PasskeyCredentialId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the user associated with this credential link.
    /// </summary>
    public User User { get; set; } = default!;

    /// <summary>
    /// Gets or sets the passkey credential associated with this link.
    /// </summary>
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
