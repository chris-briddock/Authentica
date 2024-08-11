namespace Application.Results;

/// <summary>
/// Represents the result of an operation on a shared store.
/// </summary>
public class SharedStoreResult : BaseResult<SharedStoreResult>
{
    /// <summary>
    /// Creates a successful <see cref="SharedStoreResult"/>.
    /// </summary>
    /// <returns>A <see cref="SharedStoreResult"/> indicating a successful operation.</returns>
    public static SharedStoreResult Success()
    {
        return new SharedStoreResult
        {
            Succeeded = true
        };
    }
}
