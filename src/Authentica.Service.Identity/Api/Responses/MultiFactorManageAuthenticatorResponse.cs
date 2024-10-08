namespace Api.Responses;

/// <summary>
/// Represents the response returned after managing the user's authenticator for mfa.
/// Contains the authenticator key and the QR code URI if mfa is enabled.
/// </summary>
public sealed record MultiFactorManageAuthenticatorResponse
{
    /// <summary>
    /// Gets or sets the formatted authenticator key.
    /// This key is used to set up the authenticator app for the user.
    /// </summary>
    public string? AuthenticatorKey { get; set; }

    /// <summary>
    /// Gets or sets the QR code URI.
    /// This URI can be used to generate a QR code that the user can scan with their authenticator app.
    /// </summary>
    public string? QrCodeUri { get; set; }
}
