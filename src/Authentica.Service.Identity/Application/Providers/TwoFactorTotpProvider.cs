using System.Text;
using System.Text.Encodings.Web;
using Application.Contracts;
using Domain.Aggregates.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Application.Providers;

public sealed class TwoFactorTotpProvider : ITwoFactorTotpProvider
{

    private IServiceProvider Services { get; }
    public IOptions<IdentityOptions> Options { get; }
    private UserManager<User> UserManager => Services.GetRequiredService<UserManager<User>>(); 

    public TwoFactorTotpProvider(IServiceProvider services, IOptions<IdentityOptions> options)
    {
        Services = services;
        Options = options;
    }

    public async Task<string> GenerateKeyAsync(User user)
    {
        return await Task.FromResult(UserManager.GenerateNewAuthenticatorKey());
    }

    public async Task<string> GenerateQrCodeUriAsync(User user)
    {
        var key = await GenerateKeyAsync(user);
        
        var unformattedKey = await UserManager.GetAuthenticatorKeyAsync(user);
        if (string.IsNullOrEmpty(unformattedKey))
        {
            throw new InvalidOperationException("No authenticator key found for the user.");
        }

        var email = await UserManager.GetEmailAsync(user);
        var issuer = UrlEncoder.Default.Encode(IdentityConstants.TwoFactorUserIdScheme);
        
        return $"otpauth://totp/{issuer}:{UrlEncoder.Default.Encode(email!)}?secret={unformattedKey}&issuer={issuer}&digits=6";
    }

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

    public async Task<bool> ValidateAsync(string code, User user)
    {
        return await UserManager.VerifyTwoFactorTokenAsync(
            user, UserManager.Options.Tokens.AuthenticatorTokenProvider, code);
    }
}
