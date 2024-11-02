using Domain.ValueObjects;

namespace Domain.Contracts;

/// <summary>
/// Defines a contract for tracking the modification status of an entity.
/// </summary>
/// <remarks>
/// This contract allows entities to track changes to their modification history, such as when the entity 
/// was last modified and by whom. It enables the use of generic methods in repositories that need to 
/// handle modification tracking generically.
/// </remarks>
/// <typeparam name="TKey">The type of the unique identifier for the entity, which must implement 
/// <see cref="System.IEquatable{TKey}"/> to ensure proper equality checks when accessing the modification data.</typeparam>
public interface IEntityModificationStatus<TKey> where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Gets or sets the modification status of the entity.
    /// </summary>
    /// <remarks>
    /// The <see cref="EntityModificationStatus{TKey}"/> value object encapsulates details about 
    /// the entity's modification history, such as the last modified timestamp, the user who performed the modification,
    /// and potentially other relevant metadata.
    /// </remarks>
    public EntityModificationStatus<TKey> EntityModificationStatus { get; set; }
}
