using System.Text;
using System.Text.Encodings.Web;
using Application.Contracts;
using Domain.Aggregates.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Application.Providers;

/// <summary>
/// Provides functionality for generating and validating Time-based One-Time Password (TOTP) 
/// keys for two-factor authentication.
/// </summary>
public sealed class TwoFactorTotpProvider : ITwoFactorTotpProvider
{
    /// <summary>
    /// Gets the service provider used to resolve dependencies.
    /// </summary>
    private IServiceProvider Services { get; }

    /// <summary>
    /// Gets the identity options configuration.
    /// </summary>
    public IOptions<IdentityOptions> Options { get; }

    /// <summary>
    /// Gets the user manager service from the service provider.
    /// </summary>
    private UserManager<User> UserManager => Services.GetRequiredService<UserManager<User>>(); 

    /// <summary>
    /// Initializes a new instance of the <see cref="TwoFactorTotpProvider"/> class.
    /// </summary>
    /// <param name="services">The service provider.</param>
    /// <param name="options">The identity options configuration.</param>
    public TwoFactorTotpProvider(IServiceProvider services, IOptions<IdentityOptions> options)
    {
        Services = services;
        Options = options;
    }

    /// <summary>
    /// Generates a new TOTP key for the specified user.
    /// </summary>
    /// <param name="user">The user for whom to generate the TOTP key.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the generated TOTP key.</returns>
    public async Task<string> GenerateKeyAsync(User user)
    {
        return await Task.FromResult(UserManager.GenerateNewAuthenticatorKey());
    }

    /// <summary>
    /// Generates a QR code URI for the specified user that can be used to configure an authenticator app.
    /// </summary>
    /// <param name="user">The user for whom to generate the QR code URI.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the QR code URI.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no authenticator key is found for the user.</exception>
    public async Task<string> GenerateQrCodeUriAsync(User user)
    {
        var key = await GenerateKeyAsync(user);
        
        var unformattedKey = await UserManager.GetAuthenticatorKeyAsync(user);
        if (string.IsNullOrEmpty(unformattedKey))
        {
            throw new InvalidOperationException("No authenticator key found for the user.");
        }

        var email = await UserManager.GetEmailAsync(user);
        var issuer = $"Authentica";
        
        return $"otpauth://totp/{issuer}:{UrlEncoder.Default.Encode(email!)}?secret={unformattedKey}&issuer={issuer}&digits=6";
    }

    /// <summary>
    /// Formats the unformatted TOTP key by inserting spaces for readability.
    /// </summary>
    /// <param name="unformattedKey">The unformatted TOTP key.</param>
    /// <returns>The formatted TOTP key with spaces inserted.</returns>
    public string FormatKey(string unformattedKey)
    {
        var result = new StringBuilder();
        int currentPosition = 0;
        while (currentPosition + 4 < unformattedKey.Length)
        {
            result.Append(unformattedKey.AsSpan(currentPosition, 4)).Append(' ');
            currentPosition += 4;
        }
        if (currentPosition < unformattedKey.Length)
        {
            result.Append(unformattedKey.AsSpan(currentPosition));
        }

        return result.ToString().ToLowerInvariant();
    }

    /// <summary>
    /// Validates the provided TOTP code for the specified user.
    /// </summary>
    /// <param name="code">The TOTP code to validate.</param>
    /// <param name="user">The user for whom to validate the TOTP code.</param>
    /// <returns>A task representing the asynchronous operation. The task result indicates whether the code is valid.</returns>
    public async Task<bool> ValidateAsync(string code, User user)
    {
        return await UserManager.VerifyTwoFactorTokenAsync(
            user, UserManager.Options.Tokens.AuthenticatorTokenProvider, code);
    }
}
