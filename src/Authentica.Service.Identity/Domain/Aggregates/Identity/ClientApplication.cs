using Domain.Contracts;

namespace Domain.Aggregates.Identity;

/// <summary>
/// Represents a client application in the system.
/// </summary>
public sealed class ClientApplication : ClientApplication<string>, 
                                        ISoftDeletableEntity<string>,
                                        IAuditableEntity<string>
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
    /// Gets or sets a value indicating whether the client application is deleted.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets the date and time in UTC when the client application was deleted.
    /// </summary>
    public DateTime? DeletedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who deleted this client application.
    /// </summary>
    public string? DeletedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time in UTC when the client application was created.
    /// </summary>
    public DateTime CreatedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who created this client application.
    /// </summary>
    public string CreatedBy { get; set; } = default!;

    /// <summary>
    /// Gets or sets the date and time in UTC when the client application was last modified.
    /// </summary>
    public DateTime? ModifiedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who last modified this client application.
    /// </summary>
    public string? ModifiedBy { get; set; }

    /// <summary>
    /// Gets or sets the collection of user-client application links associated with this client application.
    /// </summary>
    public ICollection<UserClientApplication> UserClientApplications { get; set; } = default!;
}

/// <summary>
/// Base class for client applications representing OAuth 2.0 clients.
/// </summary>
/// <typeparam name="TKey">The type of the unique identifier key.</typeparam>
public class ClientApplication<TKey> where TKey : IEquatable<TKey>
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
    /// A random value that should change whenever a application is persisted.
    /// </summary>
    public virtual string? ConcurrencyStamp { get; set; } = default!;
}
