namespace Application.Activities;

/// <summary>
/// Represents a base class for activities, where T is a request or response object.
/// </summary>
/// <typeparam name="T">The type of the request/response object.</typeparam>
public abstract class ActivityBase<T>
    where T : notnull
{
    /// <summary>
    /// Gets or sets the request associated with the activity.
    /// </summary>
    public virtual T Payload { get; set; } = default!;
}

/// <summary>
/// Represents a base class for activities with a request of type <typeparamref name="TRequest"/> and a response of type <typeparamref name="TResponse"/>.
/// </summary>
/// <typeparam name="TRequest">The type of the request object.</typeparam>
/// <typeparam name="TResponse">The type of the response object.</typeparam>
public abstract class ActivityBase<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : notnull
{
    /// <summary>
    /// Gets or sets the request associated with the activity.
    /// </summary>
    public virtual TRequest Request { get; set; } = default!;

    /// <summary>
    /// Gets or sets the response associated with the activity.
    /// </summary>
    public virtual TResponse Response { get; set; } = default!;
}