using Domain.ValueObjects;

namespace Domain.Contracts;

/// <summary>
/// Defines a contract for tracking the creation status of an entity.
/// </summary>
/// <remarks>
/// This contract enables the tracking of metadata related to the creation of an entity, such as who created it and when.
/// </remarks>
/// <typeparam name="TKey">The type of the unique identifier for the entity.</typeparam>
public interface IEntityCreationStatus<TKey> where TKey : IEquatable<TKey>
{
    /// <summary>
    /// The creation status of an entity.
    /// </summary>
    public EntityCreationStatus<TKey> EntityCreationStatus { get; set; }
}
