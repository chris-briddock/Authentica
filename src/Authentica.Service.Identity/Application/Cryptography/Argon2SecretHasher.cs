using Application.Contracts;

namespace Application.Cryptography;

/// <summary>
/// Implementation for a secret hasher using the Argon2 algorithm.
/// </summary>
public class Argon2SecretHasher : ISecretHasher
{
    /// <inheritdoc/>
    public string Hash(string input)
    {
        return Shared.Hash(input);
    }

    /// <inheritdoc/>
    public bool Verify(string input, string storedHash)
    {
        return Shared.Verify(input, storedHash);
    }
}