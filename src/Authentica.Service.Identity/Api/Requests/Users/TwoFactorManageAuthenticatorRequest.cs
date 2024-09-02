using Microsoft.AspNetCore.Mvc;

namespace Api.Requests;

/// <summary>
/// Represents the request to manage the user's authenticator for two-factor authentication (2FA).
/// Contains a flag indicating whether 2FA should be enabled or disabled.
/// </summary>
public sealed record TwoFactorManageAuthenticatorRequest
{
    /// <summary>
    /// Gets or sets a value indicating whether two-factor authentication (2FA) should be enabled or disabled.
    /// The value is retrieved from the query string parameter "is_enabled".
    /// </summary>
    [FromQuery(Name = "is_enabled")]
    public bool IsEnabled { get; init; } = false;
}