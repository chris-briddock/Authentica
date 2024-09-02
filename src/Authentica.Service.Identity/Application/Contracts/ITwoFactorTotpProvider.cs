using Domain.Aggregates.Identity;

namespace Application.Contracts;

/// <summary>
/// Defines the contract for a provider that handles Time-based One-Time Password (TOTP) 
/// generation, formatting, and validation for two-factor authentication.
/// </summary>
public interface ITwoFactorTotpProvider
{
    /// <summary>
    /// Generates a new TOTP key for the specified user.
    /// </summary>
    /// <param name="user">The user for whom to generate the TOTP key.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the generated TOTP key.</returns>
    Task<string> GenerateKeyAsync(User user);

    /// <summary>
    /// Generates a QR code URI for the specified user that can be used to configure an authenticator app.
    /// </summary>
    /// <param name="user">The user for whom to generate the QR code URI.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the QR code URI.</returns>
    Task<string> GenerateQrCodeUriAsync(User user);

    /// <summary>
    /// Formats the unformatted TOTP key by inserting spaces for readability.
    /// </summary>
    /// <param name="unformattedKey">The unformatted TOTP key.</param>
    /// <returns>The formatted TOTP key with spaces inserted.</returns>
    string FormatKey(string unformattedKey);

    /// <summary>
    /// Validates the provided TOTP code for the specified user.
    /// </summary>
    /// <param name="code">The TOTP code to validate.</param>
    /// <param name="user">The user for whom to validate the TOTP code.</param>
    /// <returns>A task representing the asynchronous operation. The task result indicates whether the code is valid.</returns>
    Task<bool> ValidateAsync(string code, User user);
}

