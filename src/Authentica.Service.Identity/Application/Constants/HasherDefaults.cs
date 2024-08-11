namespace Application.Constants;

/// <summary>
/// Provides default values for hashing operations.
/// </summary>
public static class HasherDefaults
{
    /// <summary>
    /// Size of the salt in bytes (512 bits).
    /// </summary>
    public const int SaltSize = 64;

    /// <summary>
    /// Size of the hash in bytes (512 bits).
    /// </summary>
    public const int HashSize = 64;

    /// <summary>
    /// Number of threads to use in parallel processing.
    /// </summary>
    public const int DegreeOfParallelism = 8; // Number of threads

    /// <summary>
    /// Memory size in kilobytes to use during hashing.
    /// </summary>
    public const int MemorySize = 1024 * 64; // Memory size in KB

    /// <summary>
    /// Number of iterations to perform in the hashing process.
    /// </summary>
    public const int Iterations = 16; // Number of iterations
}
