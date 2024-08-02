using System.Security.Cryptography;
using System.Text;
using Application.Contracts;

namespace Application.Providers;

/// <summary>
/// Represents an implementation for a random string generator.
/// </summary>
public sealed class RandomStringProvider : IRandomStringProvider
{
    /// <inheritdoc/> 
    public string Generate(int length = 256)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var sb = new StringBuilder();
        using var rng = RandomNumberGenerator.Create();

        byte[] byteArray = new byte[length];
        
        rng.GetBytes(byteArray);
        
        foreach (byte b in byteArray)
        {
            sb.Append(chars[b % chars.Length]);
        }
        byte[] sbBytes = Encoding.UTF8.GetBytes(sb.ToString());
        return Convert.ToBase64String(byteArray);
    }
}