namespace Application.Contracts;

/// <summary>
/// Represents a contract for a random string generator.
/// </summary>
public interface IRandomStringProvider
{
    /// <summary>
    /// Generates a random string of specified length.
    /// </summary>
    /// <param name="length">Length of the random string to generate. Default is 256.</param>
    /// <returns>A randomly generated string.</returns>
    string Generate(int length = 256);
}