using System.Text.Json;
using Application.Contracts;

namespace Api.Middlware;

/// <summary>
/// Middleware to enable logging of each user action as an event. 
/// </summary>
public sealed class EventLoggingMiddleware
{
    /// <summary>
    /// Delegate representing the next middleware in the request pipeline.
    /// </summary>
    private RequestDelegate Next { get; }
    /// <summary>
    /// The application service provider.
    /// </summary>
    public IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EventLoggingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the request pipeline.</param>
    /// <param name="services">The application service provider.</param>
    public EventLoggingMiddleware(RequestDelegate next,
                                  IServiceProvider services)
    {
        Next = next ?? throw new ArgumentNullException(nameof(next));
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    /// <summary>
    /// Invokes the middleware to log events.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext"/> for the current request.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        IEventStore eventStore = Services.GetRequiredService<IEventStore>();

        var @event = JsonSerializer.Serialize(context.Request.Body);

        await eventStore.SaveEventAsync(@event);

        await Next(context);
    }
}