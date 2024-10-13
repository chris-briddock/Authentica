using Domain.ValueObjects;

namespace Domain.Contracts;

/// <summary>
/// Defines a contract for a soft-deletable entity.
/// </summary>
/// <remarks>
/// This contract enables a generic method in our shared store to purge entries in the database,
/// as we need a type constraint to access the properties when representing this as a generic method. 
/// </remarks>
/// <typeparam name="TKey"></typeparam>
public interface IEntityDeletionStatus<TKey> where TKey : IEquatable<TKey>
{
    /// <summary>
    /// The deletion status of an entity.
    /// </summary>
    public EntityDeletionStatus<TKey> EntityDeletionStatus { get; set; } 
}