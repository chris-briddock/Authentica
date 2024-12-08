using Api.Requests;
using Application.Results;
using Domain.Aggregates.Identity;
using System.Security.Claims;

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
    Task<UserStoreResult> SoftDeleteUserAsync(ClaimsPrincipal user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously creates a new user based on the provided registration request.
    /// </summary>
    /// <param name="request">The registration request containing the user's details for creation.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is a <see cref="UserStoreResult"/> indicating the success or failure of the user creation operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="request"/> is null.</exception>
    /// <exception cref="OperationCanceledException">Thrown when the operation is canceled by the <paramref name="cancellationToken"/>.</exception>
    Task<UserStoreResult> CreateUserAsync(RegisterRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously confirms a user's email address using a confirmation token.
    /// </summary>
    /// <param name="user">The user whose email address is being confirmed.</param>
    /// <param name="token">The email confirmation token.</param>
    /// <returns>
    /// A <see cref="Task{UserStoreResult}"/> representing the asynchronous operation.
    /// The task result contains a <see cref="UserStoreResult"/> indicating the outcome of the operation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="user"/> or <paramref name="token"/> is null.
    /// </exception>
    /// <exception cref="Exception">Thrown if an unexpected error occurs during the operation.</exception>
    Task<UserStoreResult> ConfirmEmailAsync(User user, string token);
    /// <summary>
    /// Asynchronously resets a user's password using a reset token and a new password.
    /// </summary>
    /// <param name="user">The user whose password is being reset.</param>
    /// <param name="token">The password reset token.</param>
    /// <param name="newPassword">The new password for the user.</param>
    /// <returns>
    /// A <see cref="Task{UserStoreResult}"/> representing the asynchronous operation.
    /// The task result contains a <see cref="UserStoreResult"/> indicating the outcome of the operation.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="token"/> is null or whitespace.</exception>
    Task<UserStoreResult> ResetPasswordAsync(User user, string token, string newPassword);
    /// <summary>
    /// Asynchronously redeems a mfa recovery code for a user.
    /// </summary>
    /// <param name="user">The user attempting to redeem the recovery code.</param>
    /// <param name="code">The mfa recovery code.</param>
    /// <returns>
    /// A <see cref="Task{UserStoreResult}"/> representing the asynchronous operation.
    /// The task result contains a <see cref="UserStoreResult"/> indicating the outcome of the operation.
    /// </returns>
    /// <exception cref="Exception">Thrown if an unexpected error occurs during the operation.</exception>
    Task<UserStoreResult> RedeemMultiFactorRecoveryCodeAsync(User user, string code);
    /// <summary>
    /// Asynchronously updates a user's email address using a confirmation token.
    /// </summary>
    /// <param name="user">The user whose email address is being updated.</param>
    /// <param name="newEmail">The new email address to set for the user.</param>
    /// <param name="token">The email confirmation token.</param>
    /// <returns>
    /// A <see cref="Task{UserStoreResult}"/> representing the asynchronous operation.
    /// The task result contains a <see cref="UserStoreResult"/> indicating the outcome of the operation.
    /// </returns>
    /// <exception cref="Exception">Thrown if an unexpected error occurs during the operation.</exception>
    Task<UserStoreResult> UpdateEmailAsync(User user, string newEmail, string token);

    /// <summary>
    /// Updates the phone number of the specified user.
    /// </summary>
    /// <param name="user">The user whose phone number needs to be updated.</param>
    /// <param name="phoneNumber">The new phone number to be set for the user.</param>
    /// <param name="token">The token used to verify the phone number change.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="UserStoreResult"/> indicating the result of the operation.</returns>
    Task<UserStoreResult> UpdatePhoneNumberAsync(User user, string phoneNumber, string token);
}
