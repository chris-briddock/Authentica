namespace Api.Requests;

/// <summary>
/// Represents a two factor sign in request.
/// </summary>
public sealed record TwoFactorLoginRequest
{
    /// <summary>
    /// Gets or sets the email address
    /// </summary>
    public string EmailAddress { get; set; } = default!;
    /// <summary>
    /// The two factor token
    /// </summary>
    public string Token { get; set; } = default!;
}
