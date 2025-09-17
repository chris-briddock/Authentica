namespace Application.Extensions;

/// <summary>
/// Contains extension methods for logging operations.
/// </summary>
public static class LoggingExtensions
{
    /// <summary>
    /// Placeholder for logging extension methods.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="message">The message to log.</param>
    public static void LogCustomMessage(this ILogger logger, string message)
    {
        logger.LogInformation(message);
    }
}