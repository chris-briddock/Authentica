namespace Domain.Aggregates.Identity;

/// <summary>
/// Represents a user-passkey challenge link with a specific key type.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
public class UserPasskeyChallenge<TKey> where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Gets or sets the unique identifier for the user-client application link.
    /// </summary>
    public virtual TKey Id { get; set; } = default!;

    /// <summary>
    /// Gets or sets the identifier of the user associated with this link.
    /// </summary>
    public virtual TKey UserId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the user associated with this link.
    /// </summary>
    public User User { get; set; } = default!;

    /// <summary>
    /// Gets or sets the identifier of the passkey challenge associated with this link.
    /// </summary>
    public virtual TKey PasskeyChallengeId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the client application associated with this link.
    /// </summary>
    public virtual PasskeyChallenge PasskeyChallenge { get; set; } = default!;
}

/// <summary>
/// Represents a default implementation for the user-passkey challenge link with a string key.
/// </summary>
public sealed class UserPasskeyChallenge : UserPasskeyChallenge<string>
{
    /// <inheritdoc />
    public override string Id { get; set; } = Guid.NewGuid().ToString();
}
