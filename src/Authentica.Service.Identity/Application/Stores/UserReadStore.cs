using System.Security.Claims;
using Application.Contracts;
using Application.Factories;
using Application.Results;
using Microsoft.AspNetCore.Identity;

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

    /// <summary>
    /// Retrieves a user by their email from the given claims principal.
    /// </summary>
    /// <param name="claimsPrincipal">The claims principal containing the user's email claim.</param>
    /// <param name="cancellationToken">The cancellation token to observe.</param>
    /// <returns>A task that represents the asynchronous operation, containing the result of the operation.</returns>
    public async Task<UserStoreResult> GetUserByEmailAsync(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken = default)
    {
        try
        {
            var userClaimsPrincipal = claimsPrincipal.FindFirst(ClaimTypes.Email);

            if (userClaimsPrincipal is null)
            {
                return UserStoreResult.Failed(new IdentityError
                {
                    Code = "EmailNotFound",
                    Description = "Email claim not found in the principal."
                });
            }

            var user = await UserManager.FindByEmailAsync(userClaimsPrincipal.Value);

            if (user is null)
                return UserStoreResult.Failed(IdentityErrorFactory.UserNotFound());

            return UserStoreResult.Success(user);
        }
        catch (Exception ex)
        {
            return UserStoreResult.Failed(IdentityErrorFactory.ExceptionOccurred(ex));
        }
    }

    /// <summary>
    /// Asynchronously retrieves a user by their email address.
    /// </summary>
    /// <param name="email">The email address of the user to retrieve.</param>
    /// <returns>
    /// A <see cref="Task{UserStoreResult}"/> representing the asynchronous operation.
    /// The task result contains a <see cref="UserStoreResult"/> indicating the outcome of the operation.
    /// </returns>
    /// <exception cref="Exception">Thrown if an unexpected error occurs during the operation.</exception>
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
    /// <summary>
    /// Asynchronously retrieves a user by their id.
    /// </summary>
    /// <param name="Id">The unique identifier of the user.</param>
    /// <returns>
    /// A <see cref="Task{UserStoreResult}"/> representing the asynchronous operation.
    /// The task result contains a <see cref="UserStoreResult"/> indicating the outcome of the operation.
    /// </returns>
    /// <exception cref="Exception">Thrown if an unexpected error occurs during the operation.</exception>
    public async Task<UserStoreResult> GetUserByIdAsync(string Id)
    {
        try
        {
            var user = await UserManager.FindByIdAsync(Id);

            if (user is null)
                return UserStoreResult.Failed(IdentityErrorFactory.EmailNotFound());

            return UserStoreResult.Success(user);
        }
        catch (Exception ex)
        {
           return UserStoreResult.Failed(IdentityErrorFactory.ExceptionOccurred(ex));
        }
    }
    /// <inheritdoc />
    public async Task<IList<string>> GetUserRolesAsync(string email)
    {
        var user = await UserManager.FindByEmailAsync(email) ?? null!;
        return await UserManager.GetRolesAsync(user);
    }
}
