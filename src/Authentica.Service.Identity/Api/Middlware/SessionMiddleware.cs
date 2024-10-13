using Domain.Constants;
using Application.Contracts;
using Domain.Aggregates.Identity;
using System.Security.Claims;
using Application.Extensions;

namespace Api.Middlware;

/// <summary>
/// Middleware to ensure each HTTP session has a unique session ID.
/// </summary>
public sealed class SessionMiddleware
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
        if (!context.Session.Keys.Contains(SessionConstants.SequenceId))
        {
            var sessionId = Guid.NewGuid().ToString();
            context.Session.SetString(SessionConstants.SequenceId, sessionId);

            var scope = context.RequestServices.CreateAsyncScope();
            var sessionWriteStore = scope.ServiceProvider.GetRequiredService<ISessionWriteStore>();

            // Create a new Session object
            Session session = new()
            {
                SessionId = sessionId,
                UserId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Unknown",
                IpAddress = context.GetIpAddress(),
                UserAgent = context.Request.Headers.UserAgent.ToString(),
                Status = SessionStatus.Active,
                EntityCreationStatus = new(DateTime.UtcNow, context.User?.Identity?.Name ?? "SYSTEM"),
                EntityDeletionStatus = new(false, null, null)
            };

            // Save the session to the database
            await sessionWriteStore.CreateAsync(session);
        }

        await Next(context);
    }
}