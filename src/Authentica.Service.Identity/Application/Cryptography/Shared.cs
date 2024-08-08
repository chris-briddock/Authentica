using System.Security.Cryptography;
using System.Text;
using Application.Constants;
using Konscious.Security.Cryptography;

namespace Application.Cryptography;

/// <summary>
/// Provides shared methods for cryptographic operations.
/// </summary>
public static class Shared
{
    /// <summary>
    /// Generates a cryptographic salt using a secure random number generator.
    /// </summary>
    /// <returns>A byte array containing the generated salt.</returns>
    public static byte[] GenerateSalt()
    {
        var salt = new byte[HasherDefaults.SaltSize];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        return salt;
    }

    /// <summary>
    /// Hashes a password using the Argon2id algorithm.
    /// </summary>
    /// <param name="password">The password to be hashed.</param>
    /// <param name="salt">The salt to be used in the hashing process.</param>
    /// <returns>A byte array containing the hashed password.</returns>
    public static byte[] HashPasswordWithArgon2(string password, byte[] salt)
    {
        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            DegreeOfParallelism = HasherDefaults.DegreeOfParallelism,
            MemorySize = HasherDefaults.MemorySize,
            Iterations = HasherDefaults.Iterations
        };

        return argon2.GetBytes(HasherDefaults.HashSize);
    }
}