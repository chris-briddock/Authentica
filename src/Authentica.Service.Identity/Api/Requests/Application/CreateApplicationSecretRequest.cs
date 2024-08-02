namespace Api.Requests;

/// <summary>
/// Represents a request to create a secret for a client application.
/// </summary>
public sealed record CreateApplicationSecretRequest
{
    /// <summary>
    /// Gets or sets the name of the client application for which the secret is to be created.
    /// </summary>
    public string Name { get; set; } = default!;
}
