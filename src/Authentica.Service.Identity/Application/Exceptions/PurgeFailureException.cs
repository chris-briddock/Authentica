namespace Application.Exceptions;

/// <summary>
/// Represents errors that occur during a purge operation.
/// </summary>
public class PurgeFailureException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PurgeFailureException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public PurgeFailureException(string? message) : base(message)
    {
    }
}