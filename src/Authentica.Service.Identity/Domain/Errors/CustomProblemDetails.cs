using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Domain.Errors;

/// <summary>
/// A custom implementation of <see cref="ProblemDetails"/> 
/// that includes additional information for error handling.
/// </summary>
public sealed class CustomProblemDetails : ProblemDetails
{
    /// <summary>
    /// Gets or sets the name of the machine where the error occurred.
    /// </summary>
    [JsonPropertyName("machineName")]
    public string MachineName { get; set; } = Environment.MachineName;

    /// <summary>
    /// Gets or sets the timestamp when the error occurred.
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the request identifier for tracking purposes.
    /// </summary>
    [JsonPropertyName("requestId")]
    public string RequestId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of the exception that occurred.
    /// </summary>
    [JsonPropertyName("exceptionType")]
    public string? ExceptionType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the stack trace of the exception for debugging purposes.
    /// </summary>
    [JsonPropertyName("stackTrace")]
    public string StackTrace { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user identifier if the error is associated with a specific user.
    /// </summary>
    [JsonPropertyName("userId")]
    public string? UserId { get; set; }

    /// <summary>
    /// Gets or sets the name of the service where the error occurred.
    /// </summary>
    [JsonPropertyName("serviceName")]
    public string ServiceName { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the user agent.
    /// </summary>
    [JsonPropertyName("userAgent")]
    public string UserAgent { get; set; } = default!;
    /// <summary>
    /// Gets or sets the HTTP method used.
    /// </summary>
    [JsonPropertyName("method")]
    public string Method { get; set; } = default!;
}