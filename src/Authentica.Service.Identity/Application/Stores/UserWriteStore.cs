using System.Security.Claims;
using Api.Requests;
using Application.Contracts;
using Application.Factories;
using Application.Results;
using Domain.Aggregates.Identity;
using Domain.Contracts;
using Microsoft.EntityFrameworkCore;

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
}