using Domain.Contracts;
using Domain.ValueObjects;

namespace Domain.Aggregates.Identity;
/// <summary>
/// Represents a client application in the system.
/// </summary>
public sealed class ClientApplication : ClientApplication<string>, IEntityDeletionStatus<string>
{
    /// <summary>
    /// Gets or sets the unique identifier for the client application.
    /// </summary>
    public override string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the globally unique identifier for the client application.
    /// </summary>
    public override string ClientId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the creation status of the entity.
    /// </summary>
    /// <remarks>
    /// This property contains information about the creation of the entity.
    /// It includes whether the creation was successful and any relevant messages.
    /// </remarks>
    public EntityCreationStatus<string> EntityCreationStatus { get; set; } = default!;

    /// <summary>
    /// Gets or sets the deletion status of the entity.
    /// </summary>
    /// <remarks>
    /// This property tracks whether the entity has been soft-deleted, 
    /// along with metadata about the deletion event (like the timestamp and user responsible).
    /// </remarks>
    public EntityDeletionStatus<string> EntityDeletionStatus { get; set; } = default!;

    /// <summary>
    /// Gets or sets the modification status of the entity.
    /// </summary>
    /// <remarks>
    /// This property stores information about when the entity was created and last modified,
    /// and who performed the actions.
    /// </remarks>
    public EntityModificationStatus<string> EntityModificationStatus { get; set; } = default!;

    /// <summary>
    /// Gets or sets the collection of user-client application links associated with this client application.
    /// </summary>
    public ICollection<UserClientApplication> UserClientApplications { get; set; } = new List<UserClientApplication>();
}

/// <summary>
/// Base class for client applications representing OAuth 2.0 clients.
/// </summary>
/// <typeparam name="TKey">The type of the unique identifier key.</typeparam>
public abstract class ClientApplication<TKey> where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Gets or sets the unique identifier key for the application.
    /// </summary>
    public virtual TKey Id { get; set; } = default!;

    /// <summary>
    /// Gets or sets the globally unique identifier for the application.
    /// </summary>
    public virtual TKey ClientId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the client secret used for authentication.
    /// </summary>
    public virtual string? ClientSecret { get; set; } = default!;

    /// <summary>
    /// Gets or sets the name of the client application.
    /// </summary>
    public virtual string Name { get; set; } = default!;

    /// <summary>
    /// Gets or sets the callback uri for the client application.
    /// </summary>
    public virtual string CallbackUri { get; set; } = default!;

    /// <summary>
    /// A random value that should change whenever an application is persisted.
    /// </summary>
    public virtual string? ConcurrencyStamp { get; set; } = default!;
}