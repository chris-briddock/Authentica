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
        return Shared.Hash(password);
    }
    /// <inheritdoc/>
    public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
    {
        return Shared.Verify(providedPassword, hashedPassword) ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
    }
    
}
