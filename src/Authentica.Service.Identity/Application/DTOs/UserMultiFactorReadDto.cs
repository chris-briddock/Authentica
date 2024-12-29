namespace Application.DTOs;

/// <summary>
/// Data transfer object for user multi factor settings.
/// </summary>
public sealed class UserMultiFactorReadDto
{
    /// <summary>
    /// Gets or sets whether email multi factor authentication is enabled.
    /// </summary>
    public bool MultiFactorEmailEnabled { get; set; }

    /// <summary>
    /// Gets or sets whether authenticator multi factor authentication is enabled.
    /// </summary>
    public bool MultiFactorAuthenticatorEnabled { get; set; }

    /// <summary>
    /// Gets or sets whether passkeys multi factor authentication is enabled.
    /// </summary>
    public bool MultiFactorPasskeysEnabled { get; set; }
}
