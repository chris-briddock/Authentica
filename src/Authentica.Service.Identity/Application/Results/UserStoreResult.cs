using Domain.Aggregates.Identity;

namespace Application.Results;

/// <summary>
/// Represents the result of a user store operation.
/// </summary>
public class UserStoreResult : BaseResult<UserStoreResult>
{
    /// <summary>
    /// Gets the user associated with the result.
    /// </summary>
    public User User { get; private set; } = default!;
    
    /// <summary>
    /// Creates a successful result with the specified user.
    /// </summary>
    /// <param name="user">The user to include in the result.</param>
    /// <returns>A successful <see cref="UserStoreResult"/> with the specified user.</returns>
    public static new UserStoreResult Success(User user)
    {
        return new UserStoreResult
        {
            Succeeded = true,
            User = user
        };
    }

    /// <summary>
    /// Creates a successful result with the specified user.
    /// </summary>
    /// <returns>A successful <see cref="UserStoreResult"/> with the specified user.</returns>
    public static new UserStoreResult Success()
    {
        return new UserStoreResult
        {
            Succeeded = true
        };
    }
}
