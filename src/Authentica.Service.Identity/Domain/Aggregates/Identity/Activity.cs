namespace Domain.Aggregates.Identity;

/// <summary>
/// Represents an event with a string key.
/// </summary>
public sealed class Activity : Activity<string>
{
    /// <summary>
    /// Gets or sets the unique identifier for the event.
    /// </summary>
    public override string Id { get; set; } = Gusid.New().ToString();
}

/// <summary>
/// Represents a generic event with a key of type <typeparamref name="TKey"/>.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
public abstract class Activity<TKey> where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Gets or sets the unique identifier for the activity.
    /// </summary>
    public virtual TKey Id { get; set; } = default!;

    /// <summary>
    /// Gets or sets the session ID associated with the activity.
    /// </summary>
    public string SequenceId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the type of the activity.
    /// </summary>
    public string ActivityType { get; set; } = default!;

    /// <summary>
    /// Gets or sets the date and time when the activity was created.
    /// </summary>
    public virtual DateTime CreatedOn { get; set; } = default!;

    /// <summary>
    /// Gets or sets the request associated with the activity.
    /// </summary>
    public string? Data { get; set; } = default!;
    
    /// <summary>
    /// A random value that should change whenever the entity is persisted.
    /// </summary>
    public string ConcurrencyStamp { get; set; } = Gusid.New().ToString();
}
