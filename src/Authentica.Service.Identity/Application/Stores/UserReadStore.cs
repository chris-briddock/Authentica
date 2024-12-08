using Api.Constants;
using Application.Contracts;
using Application.Factories;
using Application.Results;
using Domain.Aggregates.Identity;
using System.Security.Claims;

namespace Application.Stores;

/// <summary>
/// Provides read operations for user-related data.
/// </summary>
public class UserReadStore : StoreBase, IUserReadStore
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserReadStore"/> class.
    /// </summary>
    /// <param name="services">The service provider for accessing application services.</param>
    public UserReadStore(IServiceProvider services) : base(services)
    {
    }

    /// <inheritdoc/>
    public async Task<UserStoreResult> GetUserByEmailAsync(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken = default)
    {
        Claim? userClaimsPrincipal = claimsPrincipal.FindFirst(ClaimTypes.Email)!;

        User? user = await UserManager.FindByEmailAsync(userClaimsPrincipal.Value);

        if (user is null)
            return UserStoreResult.Failed(IdentityErrorFactory.UserNotFound());

        return UserStoreResult.Success(user);

    }

    /// <inheritdoc/>
    public async Task<UserStoreResult> GetUserByEmailAsync(string email)
    {
        try
        {
            User? user = await UserManager.FindByEmailAsync(email);

            if (user is null)
                return UserStoreResult.Failed(IdentityErrorFactory.EmailNotFound());

            return UserStoreResult.Success(user);
        }
        catch (Exception ex)
        {
            return UserStoreResult.Failed(IdentityErrorFactory.ExceptionOccurred(ex));
        }
    }
    /// <inheritdoc/>
    public async Task<UserStoreResult> GetUserByIdAsync(string Id)
    {
        User? user = await UserManager.FindByIdAsync(Id);

        if (user is null)
            return UserStoreResult.Failed(IdentityErrorFactory.EmailNotFound());

        return UserStoreResult.Success(user);
    }
    /// <inheritdoc />
    public async Task<IList<string>> GetUserRolesAsync(string email)
    {
        User? user = await UserManager.FindByEmailAsync(email) ?? null!;
        return await UserManager.GetRolesAsync(user);
    }

    /// <inheritdoc />
    public async Task<IList<User>> GetAllUsersAsync()
    {
        IList<User> users = await UserManager.GetUsersInRoleAsync(RoleDefaults.User);
        return users;
    }
}
