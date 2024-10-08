namespace Api.Requests;

/// <summary>
/// Represents a mfa sign in request.
/// </summary>
public sealed record MultiFactorLoginRequest
{
    /// <summary>
    /// The mfa token
    /// </summary>
    public string Token { get; init; } = default!;
    /// <summary>
    /// Gets or sets whether the user would like to use an authenticator app.
    /// </summary>
    public bool UseAuthenticator { get; init; } = false;
}
