namespace Domain.ValueObjects;
/// <summary>
/// Represents the creation status of an entity.
/// </summary>
/// <typeparam name="TKey">The type of the user identifier.</typeparam>
public sealed class EntityCreationStatus<TKey> where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Gets the date and time when the entity was created in UTC.
    /// </summary>
    public DateTime CreatedOnUtc { get; set; }

    /// <summary>
    /// Gets the identifier of the user who created the entity.
    /// </summary>
    public TKey CreatedBy { get; set; }

    /// <summary>
    /// Initializes a new instance of the EntityCreationStatus class.
    /// </summary>
    /// <param name="createdOnUtc">The date and time when the entity was created in UTC.</param>
    /// <param name="createdBy">The identifier of the user who created the entity.</param>
    public EntityCreationStatus(DateTime createdOnUtc, TKey createdBy)
    {
        CreatedOnUtc = createdOnUtc;
        CreatedBy = createdBy;
    }
}