using Domain.Constants;
using Domain.Errors;
using System.Text.Json;

namespace Api.Middlware;

/// <summary>
/// Middleware for handling exceptions globally in the application.
/// </summary>
public sealed class ExceptionMiddleware
{
    /// <summary>
    /// Invokes the next middleware in the request pipeline.
    /// </summary>
    private RequestDelegate Next { get; }
    /// <summary>
    /// Logger for logging exception details.
    /// </summary>
    private ILogger<ExceptionMiddleware> Logger { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionMiddleware"/> class.
    /// </summary>
    /// <param name="logger">The logger for logging exception details.</param>
    /// <exception cref="ArgumentNullException">Thrown when next or logger is null.</exception>
    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, RequestDelegate next)
    {
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        Next = next ?? throw new ArgumentNullException(nameof(next));
    }

    /// <summary>
    /// Invokes the middleware to handle exceptions.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext"/> for the current request.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await Next(context);
        }
        catch (Exception ex)
        {
            if (!context.Response.HasStarted)
            {
                 var result = await HandleExceptionAsync(context, ex);
                Logger.LogError("An exception has occurred. {exceptionDetails}", result);
            }
        }
    }

    /// <summary>
    /// Handles the exception by creating a custom problem details response.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext"/> for the current request.</param>
    /// <param name="exception">The exception that occurred.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    private static async Task<string> HandleExceptionAsync(HttpContext context,
                                                           Exception exception)
    {

        var problemDetails = new CustomProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An exception has occurred.",
            Detail = exception.Message,
            Instance = context.Request.Path,
            MachineName = Environment.MachineName,
            Timestamp = DateTime.UtcNow,
            RequestId = context.TraceIdentifier,
            UserId = context.User?.Identity?.Name ?? "Anonymous",
            ServiceName = ServiceNameDefaults.ServiceName,
            ExceptionType = exception.GetType().Name,
            StackTrace = exception.StackTrace!,
            UserAgent = context.Request.Headers.UserAgent.ToString(),
            Method = context.Request.Method
        };

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        var result = JsonSerializer.Serialize(problemDetails);
        
        await context.Response.WriteAsync(result);

        return result;
    }
}
