using System.Security.Cryptography;
using Application.Contracts;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Application.Cryptography;

/// <summary>
/// A utility class for cryptographic operations, 
/// including encryption and verification.
/// </summary>
public class SecretHasher : ISecretHasher
{
    /// <summary>
    /// Generates a random salt.
    /// </summary>
    /// <returns>A byte array containing a random salt.</returns>
    private static byte[] GenerateSalt()
    {
        byte[] salt = new byte[512 / 8];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        return salt;
    }

    /// <inheritdoc />
    public string Hash(string input)
    {
        byte[] salt = GenerateSalt();
        byte[] hash = KeyDerivation.Pbkdf2(
            password: input,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA512,
            iterationCount: 250000,
            numBytesRequested: 512 / 8);

        return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
    }

    /// <inheritdoc />
    public bool Verify(string input, string storedHash)
    {
        string[] parts = storedHash.Split(':');
        byte[] salt = Convert.FromBase64String(parts[0]);
        byte[] hash = Convert.FromBase64String(parts[1]);

        byte[] enteredPasswordHash = KeyDerivation.Pbkdf2(
            password: input,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA512,
            iterationCount: 250000,
            numBytesRequested: 512 / 8);

        return enteredPasswordHash.SequenceEqual(hash);
    }
}