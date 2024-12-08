namespace Api.Responses;

/// <summary>
/// Represents the response for getting an application.
/// </summary>
public class ReadApplicationResponse
{
    /// <summary>
    /// Gets or sets the client ID.
    /// </summary>
    public string ClientId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the callback URI.
    /// </summary>
    public string CallbackUri { get; set; } = default!;

    /// <summary>
    /// Gets or sets the name of the client application.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Gets or sets a value indicating whether the client application is deleted.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets the date and time in UTC when the client application was deleted.
    /// </summary>
    public DateTime? DeletedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who deleted this client application.
    /// </summary>
    public string? DeletedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time in UTC when the client application was created.
    /// </summary>
    public DateTime CreatedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who created this client application.
    /// </summary>
    public string CreatedBy { get; set; } = default!;

    /// <summary>
    /// Gets or sets the date and time in UTC when the client application was last modified.
    /// </summary>
    public DateTime? ModifiedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who last modified this client application.
    /// </summary>
    public string? ModifiedBy { get; set; }
}
