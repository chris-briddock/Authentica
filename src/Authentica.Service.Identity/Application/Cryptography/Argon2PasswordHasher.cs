using Microsoft.AspNetCore.Identity;

namespace Application.Cryptography;


/// <summary>
/// Implementation class for password hashing, which integrates with ASP.NET Identity.
/// </summary>
/// <typeparam name="TUser"></typeparam>
public class Argon2PasswordHasher<TUser> : IPasswordHasher<TUser> where TUser : class
{
    /// <inheritdoc/>
     public string HashPassword(TUser user, string password)
    {
        var salt = Shared.GenerateSalt();
        var hashedPassword = Shared.HashPasswordWithArgon2(password, salt);
        return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hashedPassword)}";
    }
    /// <inheritdoc/>
    public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
    {
        var parts = hashedPassword.Split(':');
        if (parts.Length != 2)
            return PasswordVerificationResult.Failed;

        var salt = Convert.FromBase64String(parts[0]);
        var storedHashBytes = Convert.FromBase64String(parts[1]);
        var providedHash = Shared.HashPasswordWithArgon2(providedPassword, salt);

        return providedHash.SequenceEqual(storedHashBytes) ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;

    }
    
}
