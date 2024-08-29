using System.Text.Json;
using Domain.Constants;
using Domain.Errors;
using Microsoft.AspNetCore.Diagnostics;

namespace Api.Middlware;

/// <summary>
/// Middleware to intercept 4xx and 5xx response codes and return a CustomProblemDetails.
/// </summary>
public sealed class ErrorHandlingMiddleware
{
    /// <summary>
    /// Delegate representing the next middleware in the request pipeline.
    /// </summary>
    private RequestDelegate Next { get; }

    /// <summary>
    /// Logger for logging exception details.
    /// </summary>
    private ILogger<ErrorHandlingMiddleware> Logger { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorHandlingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the request pipeline.</param>
    /// <param name="logger">The logger for logging error details.</param>
    public ErrorHandlingMiddleware(RequestDelegate next,
                                   ILogger<ErrorHandlingMiddleware> logger)
    {
        Next = next ?? throw new ArgumentNullException(nameof(next));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Invokes the middleware to handle errors.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext"/> for the current request.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await Next(context);
        }
        finally
        {
            if (context.Response.StatusCode >= 400)
            {
                await HandleErrorAsync(context);
            }
        }
    }

    /// <summary>
    /// Handles the error by creating a custom problem details response.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext"/> for the current request.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    private async Task HandleErrorAsync(HttpContext context)
    {
        context.Response.ContentType = "application/problem+json";

        var problemDetails = new CustomProblemDetails
        {
            Status = context.Response.StatusCode,
            Title = GetDefaultTitleForStatusCode(context.Response.StatusCode),
            Detail = context.Features.Get<IExceptionHandlerFeature>()?.Error?.Message ?? "An error occurred processing your request.",
            Instance = context.Request.Path,
            MachineName = Environment.MachineName,
            Timestamp = DateTime.UtcNow,
            RequestId = context.TraceIdentifier,
            UserId = context.User?.Identity?.Name ?? "Anonymous",
            ServiceName = ServiceNameDefaults.ServiceName,
            StackTrace = null!,
            ExceptionType = null!,
            UserAgent = context.Request.Headers.UserAgent.ToString(),
            Method = context.Request.Method
        };

        var result = JsonSerializer.Serialize(problemDetails);
        
        Logger.LogError("An error occurred. {errorDetails}", result);

        await context.Response.WriteAsync(result);
    }

    /// <summary>
    /// Gets a default title for the given HTTP status code.
    /// </summary>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <returns>A string representing the default title for the status code.</returns>
    private static string GetDefaultTitleForStatusCode(int statusCode)
    {
        return statusCode switch
        {
            400 => "Bad Request",
            401 => "Unauthorized",
            403 => "Forbidden",
            404 => "Not Found",
            405 => "Method Not Allowed",
            500 => "Internal Server Error",
            _ => "An error occurred"
        };
    }
}
