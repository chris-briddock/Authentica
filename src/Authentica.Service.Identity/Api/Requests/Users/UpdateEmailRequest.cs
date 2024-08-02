namespace Api.Requests;

/// <summary>
/// Represents a request to update a email
/// </summary>
public sealed record UpdateEmailRequest
{
    /// <summary>
    /// Gets or sets the user's email.
    /// </summary>
    public string Email { get; set; } = default!;
    /// <summary>
    /// Gets or sets the token.
    /// </summary>
    public string Token { get; set; } = default!;
}
