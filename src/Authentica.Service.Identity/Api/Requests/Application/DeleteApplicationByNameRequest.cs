namespace Api.Requests;

/// <summary>
/// Represents a request to delete an application by its name.
/// </summary>
public sealed record DeleteApplicationByNameRequest
{
    /// <summary>
    /// Gets or initializes the name of the application to be deleted.
    /// </summary>
    public string Name { get; init; } = default!;
}