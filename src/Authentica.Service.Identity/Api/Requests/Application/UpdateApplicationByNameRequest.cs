namespace Api.Requests;

/// <summary>
/// Represents a request to create a new application.
/// </summary>
public sealed record UpdateApplicationByNameRequest
{
    /// <summary>
    /// Gets or sets the current name of the application.
    /// </summary>
    public string CurrentName { get; init; } = default!;
    /// <summary>
    /// Gets or sets the new name of the application.
    /// </summary>
    public string? NewName { get; init; } = default!;
    /// <summary>
    /// Gets or sets the new redirect uri for the application.
    /// </summary>
    public string? NewRedirectUri { get; init; } = default!;
    /// <summary>
    /// Gets or sets the new callback uri for the application.
    /// </summary>
    public string? NewCallbackUri { get; init; } = default!;

}