namespace Api.Requests;

/// <summary>
/// Represents a request to create a new application.
/// </summary>
public sealed record ReadApplicationByNameRequest
{
    /// <summary>
    /// Gets or sets the name of the application.
    /// </summary>
    public string Name { get; init; } = default!;
}