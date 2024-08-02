using System.Security.Claims;
using Api.Requests;
using Application.Results;
using Domain.Contracts;

namespace Application.Contracts;

/// <summary>
/// Defines methods for writing operations related to user data.
/// </summary>
public interface IUserWriteStore
{
    /// <summary>
    /// Soft deletes a user by marking the user as deleted in the data store.
    /// </summary>
    /// <param name="user">The user to be soft deleted.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="UserStoreResult"/> indicating the success or failure of the operation.</returns>
    Task<UserStoreResult> SoftDeleteUserAsync(ClaimsPrincipal user, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously creates a new user based on the provided registration request.
    /// </summary>
    /// <param name="request">The registration request containing the user's details for creation.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is a <see cref="UserStoreResult"/> indicating the success or failure of the user creation operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="request"/> is null.</exception>
    /// <exception cref="OperationCanceledException">Thrown when the operation is canceled by the <paramref name="cancellationToken"/>.</exception>
    Task<UserStoreResult> CreateUserAsync(RegisterRequest request, CancellationToken cancellationToken);

}
