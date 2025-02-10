using Microsoft.AspNetCore.Mvc;

namespace Domain.Requests;

/// <summary>
/// Represents a request to create a new application.
/// </summary>
public sealed record ReadApplicationByNameRequest
{
    /// <summary>
    /// Gets or sets the name of the application.
    /// </summary>
    [FromQuery(Name = "name")]
    public string Name { get; init; } = default!;
}