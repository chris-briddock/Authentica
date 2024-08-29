using Application.Attributes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Requests;

/// <summary>
/// Represents an OAuth 2.0 token request.
/// </summary>
public sealed record TokenRequest
{
    /// <summary>
    /// Gets or sets the grant type, which determines the authorization processing flow to be used.
    /// Typical values are "code", "client_credentials", "refresh_token", "device_code"
    /// </summary>
    [FromForm(Name = "grant_type")]
    public string GrantType { get; init; } = default!;

    /// <summary>
    /// Gets or sets the client identifier issued to the client during the registration process.
    /// </summary>
    [FromForm(Name = "client_id")]
    public string ClientId { get; init; } = default!;

    /// <summary>
    /// Gets or sets the client secret issued to the client during the registration process.
    /// </summary>
    [FromForm(Name = "client_secret")]
    [ProtectedPersonalData]
    [SensitiveData]
    public string ClientSecret { get; init; } = default!;

    /// <summary>
    /// Gets or sets the authorization code received from the authorization server.
    /// </summary>
    [FromForm(Name = "code")]
    public string? Code { get; init; }

    /// <summary>
    /// Gets or sets the code verifier used in the PKCE (Proof Key for Code Exchange) extension to OAuth 2.0.
    /// This is relevant for the authorization code flow.
    /// </summary>
    [FromForm(Name = "code_verifier")]
    public string? CodeVerifier { get; init; }

    /// <summary>
    /// Gets or sets the refresh token, used in the refresh token grant type.
    /// </summary>
    [FromForm(Name = "refresh_token")]
    public string? RefreshToken { get; init; }

    /// <summary>
    /// Gets or sets the device code, used in the device code grant type.
    /// </summary>
    [FromForm(Name = "device_code")]
    public string? DeviceCode { get; init; }

    /// <summary>
    /// Gets or sets the scope of the access request. 
    /// This is a space-delimited list of strings.
    /// </summary>
    [FromForm(Name = "scopes")]
    public string? Scopes { get; init; }

    /// <summary>
    /// Gets or sets the state parameter to prevent CSRF attacks.
    /// </summary> 
    [FromForm(Name = "state")]
    public string? State { get; init; } = default!;
}
