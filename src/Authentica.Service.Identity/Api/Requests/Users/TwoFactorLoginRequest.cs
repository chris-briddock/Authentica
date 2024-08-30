namespace Api.Requests;

/// <summary>
/// Represents a two factor sign in request.
/// </summary>
public sealed record TwoFactorLoginRequest
{
    /// <summary>
    /// The two factor token
    /// </summary>
    public string Token { get; init; } = default!;
    /// <summary>
    /// Gets or sets whether the user would like to use an authenticator app.
    /// </summary>
    public bool UseAuthenticator { get; init; } = false;
}
