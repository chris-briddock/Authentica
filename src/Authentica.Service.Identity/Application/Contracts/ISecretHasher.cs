namespace Application.Contracts;

/// <summary>
/// Defines a contract for hashing and verifying secrets.
/// </summary>
public interface ISecretHasher
{
    /// <summary>
    /// Computes the hash of the specified input.
    /// </summary>
    /// <param name="input">The input string to hash.</param>
    /// <returns>The hashed representation of the input string.</returns>
    string Hash(string input);

    /// <summary>
    /// Verifies that a hashed representation matches the specified input.
    /// </summary>
    /// <param name="input">The input string to verify.</param>
    /// <param name="storedHash">The stored hash to compare against.</param>
    /// <returns><c>true</c> if the input matches the stored hash; otherwise, <c>false</c>.</returns>
    bool Verify(string input, string storedHash);
}