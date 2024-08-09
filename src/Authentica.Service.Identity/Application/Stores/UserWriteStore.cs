using System.Security.Claims;
using Api.Requests;
using Application.Contracts;
using Application.Factories;
using Application.Results;
using Authentica.Common;
using Domain.Aggregates.Identity;
using Microsoft.AspNetCore.Identity;

namespace Application.Stores;

/// <summary>
/// Provides write operations for user-related data.
/// </summary>
public sealed class UserWriteStore : StoreBase, IUserWriteStore
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserWriteStore"/> class.
    /// </summary>
    /// <param name="services">The service provider to be used by the store.</param>
    public UserWriteStore(IServiceProvider services) : base(services)
    {
    }

    /// <inheritdoc/>
    public async Task<UserStoreResult> SoftDeleteUserAsync(ClaimsPrincipal user,
                                                CancellationToken cancellationToken = default)
    {
        var userReadResult = await UserReadStore.GetUserByEmailAsync(user, cancellationToken);

        var updatedUser = userReadResult.User;
        updatedUser.IsDeleted = true;
        updatedUser.DeletedBy = userReadResult.User.Id;
        updatedUser.DeletedOnUtc = DateTime.UtcNow;

        var result = await UserManager.UpdateAsync(updatedUser);

        if (!result.Succeeded)
            return UserStoreResult.Failed();

        return UserStoreResult.Success(updatedUser);

    }

    /// <inheritdoc/>
    public async Task<UserStoreResult> CreateUserAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        User user = new()
        {
            Id = Guid.NewGuid().ToString(),
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            UserName = request.Email,
            Address = request.Address

        };
        user.CreatedBy = user.Id;
        user.CreatedOnUtc = DateTime.UtcNow;
        user.PasswordHash = UserManager.PasswordHasher.HashPassword(user, $"""{request.Password}""");
        var result = await UserManager.CreateAsync(user);

        if (!result.Succeeded)
            return UserStoreResult.Failed();

        return UserStoreResult.Success(user);
    }
    /// <inheritdoc/>
    public async Task<UserStoreResult> ConfirmEmailAsync(User user, string token)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token);
        var result = await UserManager.ConfirmEmailAsync(user, token);

        if (result.Succeeded)
            return UserStoreResult.Success();

        return UserStoreResult.Failed();
    }

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
    public async Task<UserStoreResult> ResetPasswordAsync(User user, string token, string newPassword)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token);

        var tokenVerificationResult = await UserManager.VerifyUserTokenAsync(user, TokenOptions.DefaultEmailProvider, EmailTokenConstants.ResetPassword, token);

        if (!tokenVerificationResult)
            return UserStoreResult.Failed(new IdentityErrorFactory().InvalidToken());

        user.PasswordHash = UserManager.PasswordHasher.HashPassword(user, newPassword);

        return UserStoreResult.Success();
    }

    /// <summary>
    /// Asynchronously redeems a two-factor recovery code for a user.
    /// </summary>
    /// <param name="user">The user attempting to redeem the recovery code.</param>
    /// <param name="code">The two-factor recovery code.</param>
    /// <returns>
    /// A <see cref="Task{UserStoreResult}"/> representing the asynchronous operation.
    /// The task result contains a <see cref="UserStoreResult"/> indicating the outcome of the operation.
    /// </returns>
    /// <exception cref="Exception">Thrown if an unexpected error occurs during the operation.</exception>
    public async Task<UserStoreResult> RedeemTwoFactorRecoveryCodeAsync(User user, string code)
    {
        try
        {
            var result = await UserManager.RedeemTwoFactorRecoveryCodeAsync(user, code);

            if (result.Succeeded)
                return UserStoreResult.Success();

            return UserStoreResult.Failed();
        }
        catch (Exception ex)
        {
            return UserStoreResult.Failed(IdentityErrorFactory.ExceptionOccurred(ex));
        }
    }

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
    public async Task<UserStoreResult> UpdateEmailAsync(User user, string newEmail, string token)
    {
        try
        {
            var result = await UserManager.ChangeEmailAsync(user, newEmail, token);

            if (!result.Succeeded)
                return UserStoreResult.Failed();

            return UserStoreResult.Success();
        }
        catch (Exception ex)
        {
            return UserStoreResult.Failed(IdentityErrorFactory.ExceptionOccurred(ex));
        }

    }

    /// <summary>
    /// Updates the phone number of the specified user.
    /// </summary>
    /// <param name="user">The user whose phone number needs to be updated.</param>
    /// <param name="phoneNumber">The new phone number to be set for the user.</param>
    /// <param name="token">The token used to verify the phone number change.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="UserStoreResult"/> indicating the result of the operation.</returns>
    public async Task<UserStoreResult> UpdatePhoneNumberAsync(User user, string phoneNumber, string token)
    {
        try
        {
            var result = await UserManager.ChangePhoneNumberAsync(user, phoneNumber, token);

            if (!result.Succeeded)
                return UserStoreResult.Failed();

            return UserStoreResult.Success();
        }
        catch (Exception ex)
        {
            return UserStoreResult.Failed(IdentityErrorFactory.ExceptionOccurred(ex));
        }
    }
}