namespace Application.Results;

/// <summary>
/// Represents the result of an operation performed in the application store context.
/// </summary>
public sealed class ApplicationStoreResult : BaseResult<ApplicationStoreResult>
{
    /// <summary>
    /// Gets or sets the secret associated with the application.
    /// </summary>
    public string? Secret { get; private set; } = default!;

    /// <summary>
    /// Creates a successful result with the specified app.
    /// </summary>
    /// <param name="secret">The secret to include in the result.</param>
    /// <returns>A successful <see cref="ApplicationStoreResult"/> with the specified user.</returns>
    public static ApplicationStoreResult Success(string secret)
    {
        return new ApplicationStoreResult
        {
            Succeeded = true,
            Secret = secret
        };
    }
}
