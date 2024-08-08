namespace Application.Constants;

/// <summary>
/// Provides default values for hashing operations.
/// </summary>
public static class HasherDefaults
{
    /// <summary>
    /// Size of the salt in bytes (128 bits).
    /// </summary>
    public const int SaltSize = 128 / 8;

    /// <summary>
    /// Size of the hash in bytes (256 bits).
    /// </summary>
    public const int HashSize = 256 / 8;

    /// <summary>
    /// Number of threads to use in parallel processing.
    /// </summary>
    public const int DegreeOfParallelism = 8; // Number of threads

    /// <summary>
    /// Memory size in kilobytes to use during hashing.
    /// </summary>
    public const int MemorySize = 1024 * 2; // Memory size in KB

    /// <summary>
    /// Number of iterations to perform in the hashing process.
    /// </summary>
    public const int Iterations = 16; // Number of iterations
}
