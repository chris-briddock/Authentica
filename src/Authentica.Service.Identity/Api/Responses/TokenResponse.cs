using Application.Attributes;

namespace Api.Responses;

/// <summary>
/// Represents the response when a user is authorized.
/// </summary>
public sealed class TokenResponse
{
    /// <summary>
    /// Gets or sets the access token.
    /// </summary>
    [SensitiveData]
    public string AccessToken { get; set; } = default!;
    /// <summary>
    /// Gets or sets the refresh token.
    /// </summary>
    [SensitiveData]
    public string RefreshToken { get; set; } = default!;
    /// <summary>
    /// Gets or sets the expiration of the token.
    /// </summary>
    public string Expires { get; set; } = default!;
    /// <summary>
    /// Gets or sets the token type.
    /// </summary>
    public string TokenType { get; set; } = default!;
}