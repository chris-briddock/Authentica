namespace Api.Requests;

/// <summary>
/// Represents a request to create a new application.
/// </summary>
public sealed record CreateApplicationRequest
{
    /// <summary>
    /// Gets or sets the name of the application.
    /// </summary>
    public string Name { get; init; } = default!;
    /// <summary>
    /// Gets or sets the callback uri for the application.
    /// </summary>
    public string CallbackUri { get; init; } = default!;

}