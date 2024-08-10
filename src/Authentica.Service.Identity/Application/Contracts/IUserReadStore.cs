using System.Security.Claims;
using Application.Results;
using Domain.Aggregates.Identity;

namespace Application.Contracts;

/// <summary>
/// Defines a contract for user read operations.
/// </summary>
public interface IUserReadStore
{
    /// <summary>
    /// Retrieves a user by their email from the given claims principal.
    /// </summary>
    /// <param name="claimsPrincipal">The claims principal containing the user's email claim.</param>
    /// <param name="cancellationToken">The cancellation token to observe.</param>
    /// <returns>A task that represents the asynchronous operation, containing the result of the operation.</returns>
    /// <exception cref="Exception">Thrown if an unexpected error occurs during the operation.</exception>
    Task<UserStoreResult> GetUserByEmailAsync(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves a user by their email address.
    /// </summary>
    /// <param name="email">The email address of the user to retrieve.</param>
    /// <returns>
    /// A <see cref="Task{UserStoreResult}"/> representing the asynchronous operation.
    /// The task result contains a <see cref="UserStoreResult"/> indicating the outcome of the operation.
    /// </returns>
    /// <exception cref="Exception">Thrown if an unexpected error occurs during the operation.</exception>
    Task<UserStoreResult> GetUserByEmailAsync(string email);
    /// <summary>
    /// Asynchronously retrieves a user by their id.
    /// </summary>
    /// <param name="Id">The unique identifier of the user.</param>
    /// <returns>
    /// A <see cref="Task{UserStoreResult}"/> representing the asynchronous operation.
    /// The task result contains a <see cref="UserStoreResult"/> indicating the outcome of the operation.
    /// </returns>
    /// <exception cref="Exception">Thrown if an unexpected error occurs during the operation.</exception>
    Task<UserStoreResult> GetUserByIdAsync(string Id);
    /// <summary>
    /// Asynchronously retrieves the list of roles associated with a specified user.
    /// </summary>
    /// <param name="email">The email of the user for whom to retrieve roles.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of role names associated with the user.
    /// </returns>
    Task<IList<string>> GetUserRolesAsync(string email);
}
