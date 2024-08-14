namespace Domain.Constants;

/// <summary>
/// Represents the authorize response types
/// </summary>
public static class TokenConstants
{
    /// <summary>
    /// Represents the value for client credentials token type.
    /// </summary>
    public const string ClientCredentials = "client_credentials";
    /// <summary>
    /// Represents the value for device code token type.
    /// </summary>
    public const string DeviceCode = "device_code";

    /// <summary>
    /// Represents the value for the authorization code token type.
    /// </summary>
    public const string AuthorizationCode = "code";

    /// <summary>
    /// Represents the value for the refresh token type.
    /// </summary>
    public const string Refresh = "refresh_token";
}