using Microsoft.AspNetCore.Mvc;

namespace Api.Requests;

/// <summary>
/// Represents the request to manage the user's authenticator for mfa.
/// </summary>
public sealed record MultiFactorManageAuthenticatorRequest
{
    /// <summary>
    /// Gets or sets a value indicating whether mfa should be enabled or disabled.
    /// The value is retrieved from the query string parameter "is_enabled".
    /// </summary>
    [FromQuery(Name = "is_enabled")]
    public bool IsEnabled { get; init; } = false;
}