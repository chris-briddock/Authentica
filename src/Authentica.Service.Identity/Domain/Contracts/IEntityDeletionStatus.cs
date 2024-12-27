using Domain.ValueObjects;

namespace Domain.Contracts;

/// <summary>
/// Defines a contract for an entity that supports soft deletion.
/// </summary>
/// <remarks>
/// This contract is used to allow soft deletion functionality for entities, where an entity is marked 
/// as deleted but remains in the database. It also enables the use of generic methods in shared 
/// repositories, where a type constraint is needed to access the deletion status properties.
/// </remarks>
/// <typeparam name="TKey">The type of the unique identifier for the entity, which must implement 
/// <see cref="IEquatable{TKey}"/> for proper equality checks in deletion operations.</typeparam>
public interface IEntityDeletionStatus<TKey> where TKey : IEquatable<TKey>
{
    /// <summary>
    /// The deletion status of an entity.
    /// </summary>
    public EntityDeletionStatus<TKey> EntityDeletionStatus { get; set; }
}