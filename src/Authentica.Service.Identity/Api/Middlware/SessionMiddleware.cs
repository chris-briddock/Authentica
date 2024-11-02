using Domain.Constants;
using Domain.Aggregates.Identity;
using System.Security.Claims;
using Application.Extensions;
using Authentica.Service.Identity.Application.Contracts.Stores;
using Application.Contracts;

namespace Api.Middlware;

/// <summary>
/// Middleware to ensure each HTTP session has a unique session ID.
/// </summary>
public sealed class SessionMiddleware
{
    private static readonly SemaphoreSlim _semaphore = new(1, 1);

    /// <summary>
    /// Invokes the next middleware in the request pipeline.
    /// </summary>
    private RequestDelegate Next { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="SessionMiddleware"/>
    /// </summary>
    /// <param name="next">Invokes the next middleware in the request pipeline.</param>
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
        string? emailAddress = context.User.FindFirst(ClaimTypes.Email)?.Value;
        bool shouldCreate = false;
        using AsyncServiceScope scope = context.RequestServices.CreateAsyncScope();

        try
        {
            await _semaphore.WaitAsync();
            string? currentSessionId = context.Session.GetString(SessionConstants.SequenceId);
            var sessionReadStore = scope.ServiceProvider.GetRequiredService<ISessionReadStore>();

            if (currentSessionId is null)
                shouldCreate = true;

            if (currentSessionId is not null)
            {
                Session? storedSession = await sessionReadStore.GetByIdAsync(currentSessionId);

                if (storedSession is not null &&
                    storedSession.UserId == "Unknown")
                    shouldCreate = true;

            }

            if (shouldCreate)
            {
                string? userId = null!;    
                var sessionId = Guid.NewGuid().ToString();
                context.Session.SetString(SessionConstants.SequenceId, sessionId);
                var sessionWriteStore = scope.ServiceProvider.GetRequiredService<ISessionWriteStore>();
                var userReadStore = scope.ServiceProvider.GetRequiredService<IUserReadStore>();

                if (emailAddress is not null)
                    userId = (await userReadStore.GetUserByEmailAsync(emailAddress)).User.Id;

                // Create a new Session object
                Session session = new()
                {
                    SessionId = sessionId,
                    UserId = userId ?? "Unknown",
                    IpAddress = context.GetIpAddress(),
                    UserAgent = context.Request.Headers.UserAgent.ToString(),
                    Status = SessionStatus.Active,
                    EntityCreationStatus = new(DateTime.UtcNow, context.User?.Identity?.Name ?? "SYSTEM"),
                    EntityDeletionStatus = new(false, null, null)
                };

                // Save the session to the database
                await sessionWriteStore.CreateAsync(session);
            }

        }
        finally
        {
            _semaphore.Release();
        }

        await Next(context);
    }
}
