using Domain.Aggregates.Identity;

namespace Application.Contracts;

public interface ITwoFactorTotpProvider
{
    Task<string> GenerateKeyAsync(User user);
    Task<string> GenerateQrCodeUriAsync(User user);
    string FormatKey(string unformattedKey);
    Task<bool> ValidateAsync(string code, User user);
}
