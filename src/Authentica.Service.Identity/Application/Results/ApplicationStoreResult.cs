using Domain.Aggregates.Identity;
using MongoDB.Driver.Core.Events;

namespace Application.Results;

/// <summary>
/// Represents the result of an operation performed in the application store context.
/// </summary>
public class ApplicationStoreResult : BaseResult<ApplicationStoreResult>
{
    /// <summary>
    /// 
    /// </summary>  
    public string? Secret { get; private set; } = default!;

    /// <summary>
    /// Gets or sets the client application associated with the result.
    /// </summary> 
    public ClientApplication? Application { get; private set; }

    /// <summary>
    /// Creates a successful result with the specified app.
    /// </summary>
    /// <param name="secret">The secret to include in the result.</param>
    /// <returns>A successful <see cref="ApplicationStoreResult"/> with the specified user.</returns>
    public static new ApplicationStoreResult Success(string secret)
    {
        return new ApplicationStoreResult
        {
            Succeeded = true,
            Secret = secret
        };
    }

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    /// <returns>A successful <see cref="ApplicationStoreResult"/> with the specified user.</returns>
    public static new ApplicationStoreResult Success()
    {
        return new ApplicationStoreResult
        {
            Succeeded = true,
        };
    }
}
