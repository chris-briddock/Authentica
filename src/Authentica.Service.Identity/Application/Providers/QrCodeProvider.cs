using System.Text;
using System.Text.Encodings.Web;
using Application.Contracts;
using Domain.Constants;

namespace Application.Providers;

public sealed class QrCodeProvider : IQrCodeProvider
{
    public string FormatKey(string unformattedKey)
    {
        var result = new StringBuilder();
        int currentPosition = 0;
        while (currentPosition + 4 < unformattedKey.Length)
        {
            result.Append(unformattedKey.Substring(currentPosition, 4)).Append(' ');
            currentPosition += 4;
        }
        if (currentPosition < unformattedKey.Length)
        {
            result.Append(unformattedKey.AsSpan(currentPosition));
        }

        return result.ToString().ToLowerInvariant();
    }

    public string Generate(string email, string unformattedKey)
    {
        const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
        return string.Format(
            AuthenticatorUriFormat,
            UrlEncoder.Default.Encode(ServiceNameDefaults.ServiceName),
            UrlEncoder.Default.Encode(email),
            unformattedKey);
    }
}
