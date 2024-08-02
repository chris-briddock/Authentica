using Domain.Constants;

namespace Api.Middlware;

/// <summary>
/// Middleware to ensure each HTTP session has a unique session ID.
/// </summary>
public class SessionMiddleware
{
    /// <summary>
    /// Delegate representing the next middleware in the request pipeline.
    /// </summary>
    private RequestDelegate Next { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SessionMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the request pipeline.</param>
    public SessionMiddleware(RequestDelegate next)
    {
        Next = next;
    }

    /// <summary>
    /// Processes an HTTP request to ensure it has a unique session ID.
    /// </summary>
    /// <param name="context">The HTTP context for the current request.</param>
    /// <returns>A task that represents the completion of request processing.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        // Check if the session does not already contain a SequenceId.
        if (!context.Session.Keys.Contains(SessionConstants.SequenceId))
        {
            // Generate a new guid and set it in the session
            context.Session.SetString(SessionConstants.SequenceId, Guid.NewGuid().ToString());
        }

        // Call the next middleware in the pipeline
        await Next(context);
    }
}