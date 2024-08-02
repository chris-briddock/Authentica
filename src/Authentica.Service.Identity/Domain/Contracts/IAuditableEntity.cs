namespace Domain.Contracts;

/// <summary>
/// Defines a contract for auditable entities.
/// </summary>
public interface IAuditableEntity<TKey> where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Represents the created date and time for an auditable entity.
    /// </summary>
    public DateTime CreatedOnUtc { get; }
    /// <summary>
    /// Represents who modified the user.
    /// </summary>
    public TKey CreatedBy { get; }
    /// <summary>
    /// Represents the modified date and time for an auditable entity.
    /// </summary>
    public DateTime? ModifiedOnUtc { get; }
    /// <summary>
    /// Represents who modified the user.
    /// </summary>
    public TKey? ModifiedBy { get; }
}