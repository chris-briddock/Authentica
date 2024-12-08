using Application.Constants;
using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

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
    /// <summary>
    /// Hashes a given string with the Argon2 algorithm.
    /// </summary>
    /// <param name="input">The string to hash.</param>
    /// <returns>The hashed string</returns>
    public static string Hash(string input)
    {
        var salt = GenerateSalt();
        var hashedPassword = HashPasswordWithArgon2(input, salt);

        // Combine salt and hashed password
        byte[] saltAndHash = new byte[salt.Length + hashedPassword.Length];
        Buffer.BlockCopy(salt, 0, saltAndHash, 0, salt.Length);
        Buffer.BlockCopy(hashedPassword, 0, saltAndHash, salt.Length, hashedPassword.Length);

        return Convert.ToBase64String(saltAndHash);
    }
    /// <summary>
    /// Verifies the password by re-encrypting the input, and comparing the stored hash.
    /// </summary>
    /// <param name="input">The input string.</param>
    /// <param name="storedHash">The stored password hash.</param>
    /// <returns></returns>
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