using Domain.ValueObjects;

namespace Application.DTOs;

/// <summary>
/// Represents a data transfer object for reading application details.
/// </summary>
public sealed class ApplicationReadDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the client application.
    /// </summary>
    public required string ClientId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the status of entity deletion, including any relevant metadata.
    /// </summary>
    public required EntityDeletionStatus<string> EntityDeletionStatus { get; set; } = default!;

    /// <summary>
    /// Gets or sets the status of entity creation, including any relevant metadata.
    /// </summary>
    public required EntityCreationStatus<string> EntityCreationStatus { get; set; } = default!;

    /// <summary>
    /// Gets or sets the status of entity modification, including any relevant metadata.
    /// </summary>
    public required EntityModificationStatus<string> EntityModificationStatus { get; set; } = default!;

    /// <summary>
    /// Gets or sets the URI that the application will use for callbacks.
    /// </summary>
    public required string CallbackUri { get; set; } = default!;

    /// <summary>
    /// Gets or sets the name of the application.
    /// </summary>
    public required string Name { get; set; } = default!;

    /// <summary>
    /// Gets or sets the user ID associated with the application.
    /// </summary>
    public string? UserId { get; set; } = default!;
    /// <summary>
    /// Gets or sets the client secret associated with the application.
    /// </summary>
    public string? ClientSecret { get; set; } = default!;
}
