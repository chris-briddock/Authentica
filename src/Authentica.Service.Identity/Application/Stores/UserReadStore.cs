using System.Security.Claims;
using Api.Constants;
using Application.Contracts;
using Application.Factories;
using Application.Results;
using Domain.Aggregates.Identity;

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
        var userClaimsPrincipal = claimsPrincipal.FindFirst(ClaimTypes.Email)!;

        var user = await UserManager.FindByEmailAsync(userClaimsPrincipal.Value);

        if (user is null)
            return UserStoreResult.Failed(IdentityErrorFactory.UserNotFound());

        return UserStoreResult.Success(user);

    }

    /// <inheritdoc/>
    public async Task<UserStoreResult> GetUserByEmailAsync(string email)
    {
        try
        {
            var user = await UserManager.FindByEmailAsync(email);

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
        var user = await UserManager.FindByIdAsync(Id);

        if (user is null)
            return UserStoreResult.Failed(IdentityErrorFactory.EmailNotFound());

        return UserStoreResult.Success(user);
    }
    /// <inheritdoc />
    public async Task<IList<string>> GetUserRolesAsync(string email)
    {
        var user = await UserManager.FindByEmailAsync(email) ?? null!;
        return await UserManager.GetRolesAsync(user);
    }

    /// <summary>
    /// Asynchronously retrieves a list of all users in the specified role.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of users in the specified role.
    /// </returns>
    public async Task<IList<User>> GetAllUsersAsync()
    {
        var users = await UserManager.GetUsersInRoleAsync(RoleDefaults.User);
        return users;
    }
}
