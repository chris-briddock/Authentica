namespace Application.Extensions;

/// <summary>
/// Provides extension methods for validation.
/// </summary>
public static class ValidationExtensions
{
    /// <summary>
    /// Determines whether the specified string is a valid URI.
    /// </summary>
    /// <param name="uri">The string to validate as a URI.</param>
    /// <returns><c>true</c> if the specified string is a valid URI; otherwise, <c>false</c>.</returns>
    public static bool BeAValidUri(this string uri)
    {
        return Uri.TryCreate(uri, UriKind.Absolute, out _);
    }

    /// <summary>
    /// Determines whether the specified string is a valid GUID.
    /// </summary>
    /// <param name="guid">The string to validate as a GUID.</param>
    /// <returns><c>true</c> if the specified string is a valid GUID; otherwise, <c>false</c>.</returns>
    public static bool BeAValidGuid(this string guid)
    {
        return Guid.TryParse(guid, out _);
    }

    /// <summary>
    /// Determines whether the specified string is a valid OAuth 2.0 grant type.
    /// </summary>
    /// <param name="grantType">The grant type string to validate.</param>
    /// <returns><c>true</c> if the specified string is a valid grant type; otherwise, <c>false</c>.</returns>
    public static bool BeAValidGrantType(this string grantType)
    {
        var validGrantTypes = new[] { "code", "client_credentials", "refresh_token", "device_code" };
        return Array.Exists(validGrantTypes, type => type.Equals(grantType, StringComparison.OrdinalIgnoreCase));
    }
}
