namespace Domain.ValueObjects;

/// <summary>
/// Represents the modification status of an entity.
/// </summary>
public sealed class EntityModificationStatus<TKey> where TKey : IEquatable<TKey>
{
    /// <summary>
    /// The date and time when the entity was last modified in UTC.
    /// </summary>
    public DateTime? ModifiedOnUtc { get; set; }

    /// <summary>
    /// The identifier of the user who last modified the entity.
    /// </summary>
    public TKey? ModifiedBy { get; set; }

    /// <summary>
    /// Initializes a new instance of <see cref="EntityModificationStatus{TKey}"/>
    /// </summary>
    /// <param name="modifiedOnUtc">The date and time when the entity was last modified in UTC. Null if never modified.</param>
    /// <param name="modifiedBy">The identifier of the user who last modified the entity. Null if never modified.</param>
    public EntityModificationStatus(DateTime? modifiedOnUtc, TKey? modifiedBy)
    {
        ModifiedOnUtc = modifiedOnUtc;
        ModifiedBy = modifiedBy;
    }
}