namespace Api.Requests;

/// <summary>
/// Represents a request to delete a session.
/// </summary>
public sealed record DeleteSessionRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the session to be deleted.
    /// </summary>
    public string SessionId { get; set; } = default!;
}