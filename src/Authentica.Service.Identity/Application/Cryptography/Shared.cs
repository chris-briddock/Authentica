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
    private static byte[] GenerateSalt()
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
    private static byte[] HashPasswordWithArgon2(string password, byte[] salt)
    {
        var argon2 = new Argon2d(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            DegreeOfParallelism = HasherDefaults.DegreeOfParallelism,
            MemorySize = HasherDefaults.MemorySize,
            Iterations = HasherDefaults.Iterations
        };

        return argon2.GetBytes(HasherDefaults.HashSize);
    }

    public static string Hash(string password)
    {
        var salt = GenerateSalt();
        var hashedPassword = Shared.HashPasswordWithArgon2(password, salt);

        // Combine salt and hashed password
        byte[] saltAndHash = new byte[salt.Length + hashedPassword.Length];
        Buffer.BlockCopy(salt, 0, saltAndHash, 0, salt.Length);
        Buffer.BlockCopy(hashedPassword, 0, saltAndHash, salt.Length, hashedPassword.Length);

        return Convert.ToBase64String(saltAndHash);
    }

    public static bool Verify(string input, string storedHash)
    {
        byte[] saltAndHash = Convert.FromBase64String(storedHash);

        // Extract salt and hash from combined byte array
        int saltLength = HasherDefaults.SaltSize; // Assuming Shared has a SaltLength property or constant
        byte[] salt = new byte[saltLength];
        byte[] storedHashBytes = new byte[saltAndHash.Length - saltLength];

        Buffer.BlockCopy(saltAndHash, 0, salt, 0, salt.Length);
        Buffer.BlockCopy(saltAndHash, salt.Length, storedHashBytes, 0, storedHashBytes.Length);

        byte[] providedHash = HashPasswordWithArgon2(input, salt);

        return providedHash.SequenceEqual(storedHashBytes);
    }
}