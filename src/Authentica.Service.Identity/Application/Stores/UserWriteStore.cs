using System.Security.Claims;
using Api.Requests;
using Application.Contracts;
using Application.Results;
using Domain.Aggregates.Identity;

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

        var result = await UserManager.ResetPasswordAsync(user, token, newPassword);

        if (result.Succeeded)
            return UserStoreResult.Success();

        return UserStoreResult.Failed();
    }
}