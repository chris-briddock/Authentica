using Application.Contracts;

namespace Application.Cryptography;

/// <summary>
/// Implementation for a secret hasher using the Argon2 algorithm.
/// </summary>
public class Argon2SecretHasher : ISecretHasher
{
    /// <inheritdoc/>
    public string Hash(string password)
    {
        var salt = Shared.GenerateSalt();
        var hashedPassword = Shared.HashPasswordWithArgon2(password, salt);
        return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hashedPassword)}";
    }

    /// <inheritdoc/>
    public bool Verify(string input, string storedHash)
    {
        var parts = storedHash.Split(':');
        if (parts.Length != 2)
            return false;

        var salt = Convert.FromBase64String(parts[0]);
        var storedHashBytes = Convert.FromBase64String(parts[1]);
        var providedHash = Shared.HashPasswordWithArgon2(input, salt);

        return providedHash.SequenceEqual(storedHashBytes);
    }
}