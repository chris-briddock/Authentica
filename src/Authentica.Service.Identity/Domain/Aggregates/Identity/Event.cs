namespace Domain.Aggregates.Identity;

/// <summary>
/// Represents an event with a string key.
/// </summary>
public sealed class Event : Event<string>
{
    /// <summary>
    /// Gets or sets the unique identifier for the event.
    /// </summary>
    public override string Id { get; set; } = Guid.NewGuid().ToString();
}

/// <summary>
/// Represents a generic event with a key of type <typeparamref name="TKey"/>.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
public class Event<TKey> where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Gets or sets the unique identifier for the event.
    /// </summary>
    public virtual TKey Id { get; set; } = default!;

    /// <summary>
    /// Gets or sets the session ID associated with the event.
    /// </summary>
    public string SequenceId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the type of the event.
    /// </summary>
    public string EventType { get; set; } = default!;

    /// <summary>
    /// Gets or sets the date and time when the event was created.
    /// </summary>
    public virtual DateTime CreatedOn { get; set; } = default!;

    /// <summary>
    /// Gets or sets the request associated with the event.
    /// </summary>
    public string? Data { get; set; } = default!;
}
