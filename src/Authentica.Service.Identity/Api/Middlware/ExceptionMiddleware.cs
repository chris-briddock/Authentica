using Application.Exceptions;
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
    /// <param name="next">Invokes the next middleware in the pipeline.</param>
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
        catch (ArgumentNullException ex)
        {
            if (!context.Response.HasStarted)
            {
                var result = await HandleExceptionAsync(context, ex);
                Logger.LogError("An argument null exception has occurred. {ExceptionDetails}", result);
            }
        }
        catch (ArgumentException ex)
        {
            if (!context.Response.HasStarted)
            {
                var result = await HandleExceptionAsync(context, ex);
                Logger.LogError("An argument exception has occurred. {ExceptionDetails}", result);
            }
        }
        catch (InvalidOperationException ex)
        {
            if (!context.Response.HasStarted)
            {
                var result = await HandleExceptionAsync(context, ex);
                Logger.LogError("An invalid operation exception has occurred. {ExceptionDetails}", result);
            }
        }
        catch (NotSupportedException ex)
        {
            if (!context.Response.HasStarted)
            {
                var result = await HandleExceptionAsync(context, ex);
                Logger.LogError("A not supported exception has occurred. {ExceptionDetails}", result);
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            if (!context.Response.HasStarted)
            {
                var result = await HandleExceptionAsync(context, ex);
                Logger.LogError("An unauthorized access exception has occurred. {ExceptionDetails}", result);
            }
        }
        catch (Application.Exceptions.PurgeFailureException ex)
        {
            if (!context.Response.HasStarted)
            {
                var result = await HandleExceptionAsync(context, ex);
                Logger.LogError("A purge failure exception has occurred. {ExceptionDetails}", result);
            }
        }
        catch (System.Security.SecurityException ex)
        {
            if (!context.Response.HasStarted)
            {
                var result = await HandleExceptionAsync(context, ex);
                Logger.LogError("A security exception has occurred. {ExceptionDetails}", result);
            }
        }
        catch (Exception ex)
        {
            if (!context.Response.HasStarted)
            {
                var result = await HandleExceptionAsync(context, ex);
                Logger.LogError("An unexpected exception has occurred. {ExceptionDetails}", result);
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
