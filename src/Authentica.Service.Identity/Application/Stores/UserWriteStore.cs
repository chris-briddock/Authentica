using Api.Requests;
using Application.Contracts;
using Application.Factories;
using Application.Results;
using Authentica.Common;
using Domain.Aggregates.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

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
        var updatedUser = (await UserReadStore.GetUserByEmailAsync(user, cancellationToken)).User;

        updatedUser.EntityDeletionStatus = new(true, DateTime.UtcNow, updatedUser.Id);

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
        user.EntityCreationStatus = new(DateTime.UtcNow, user.Id);
        user.EntityModificationStatus = new(DateTime.UtcNow, user.Id);
        user.EntityDeletionStatus = new(false, null, null);
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
        var result = await UserManager.VerifyUserTokenAsync(user, TokenOptions.DefaultEmailProvider, EmailTokenConstants.ConfirmEmail, token);

        if (!result)
            return UserStoreResult.Failed();

        user.EmailConfirmed = true;

        await UserManager.UpdateAsync(user);

        return UserStoreResult.Success();
    }

    /// <inheritdoc/>
    public async Task<UserStoreResult> ResetPasswordAsync(User user, string token, string newPassword)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token);

        var tokenVerificationResult = await UserManager.VerifyUserTokenAsync(user, TokenOptions.DefaultEmailProvider, EmailTokenConstants.ResetPassword, token);
        var passwordHasher = Services.GetRequiredService<IPasswordHasher<User>>();

        if (!tokenVerificationResult)
            return UserStoreResult.Failed(new IdentityErrorFactory().InvalidToken());

        user.PasswordHash = passwordHasher.HashPassword(user, newPassword);

        await UserManager.UpdateAsync(user);

        return UserStoreResult.Success();
    }

    /// <inheritdoc/>
    public async Task<UserStoreResult> RedeemMultiFactorRecoveryCodeAsync(User user, string code)
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

    /// <inheritdoc/>
    public async Task<UserStoreResult> UpdateEmailAsync(User user, string newEmail, string token)
    {
        try
        {
            var result = await UserManager.VerifyUserTokenAsync(user, TokenOptions.DefaultEmailProvider, EmailTokenConstants.UpdateEmail, token);

            if (!result)
                return UserStoreResult.Failed();

            user.Email = newEmail;
            user.NormalizedEmail = newEmail.ToUpper();
            user.UserName = newEmail;
            user.NormalizedEmail = newEmail.ToUpper();

            await UserManager.UpdateAsync(user);

            return UserStoreResult.Success();
        }
        catch (Exception ex)
        {
            return UserStoreResult.Failed(IdentityErrorFactory.ExceptionOccurred(ex));
        }

    }

    /// <inheritdoc/>
    public async Task<UserStoreResult> UpdatePhoneNumberAsync(User user, string phoneNumber, string token)
    {
        try
        {
            var result = await UserManager.VerifyUserTokenAsync(user, TokenOptions.DefaultEmailProvider, EmailTokenConstants.UpdatePhoneNumber, token);

            if (!result)
                return UserStoreResult.Failed();

            user.PhoneNumber = phoneNumber;

            await UserManager.UpdateAsync(user);

            return UserStoreResult.Success();
        }
        catch (Exception ex)
        {
            return UserStoreResult.Failed(IdentityErrorFactory.ExceptionOccurred(ex));
        }
    }


}