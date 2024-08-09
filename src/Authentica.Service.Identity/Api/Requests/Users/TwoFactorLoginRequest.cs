namespace Api.Requests;

/// <summary>
/// Represents a two factor sign in request.
/// </summary>
public sealed record TwoFactorLoginRequest
{
    /// <summary>
    /// The two factor token
    /// </summary>
    public string Token { get; set; } = default!;
}
