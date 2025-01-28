using Api.Constants;
using Application.Constants;
using Application.Factories;
using Application.Results;
using Domain.Aggregates.Identity;
using Domain.Contracts.Stores;
using System.Security.Claims;
using ZiggyCreatures.Caching.Fusion;

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
    public async Task<UserStoreResult> GetUserByEmailAsync(ClaimsPrincipal claimsPrincipal,
                                                           CancellationToken cancellationToken = default)
    {
        var emailClaim = claimsPrincipal.FindFirst(ClaimTypes.Email);

        if (emailClaim is null)
            return UserStoreResult.Failed(IdentityErrorFactory.EmailNotFound());

        var email = emailClaim.Value;

        // Use FusionCache to manage caching
        var cacheKey = $"user_email_{email}";  // Cache key based on the email

        var user = await FusionCache.GetOrSetAsync<User>(
            cacheKey,
            async (ctx, ct) =>
            {
                ctx.Tags = [CacheTagConstants.Users];
                User? user = await UserManager.FindByEmailAsync(email);
                // Query the database if the value is not found in the cache
                return user!;
            },
            token: cancellationToken
        );

        if (user is null)
            return UserStoreResult.Failed(IdentityErrorFactory.UserNotFound());

        return UserStoreResult.Success(user);
    }

    /// <inheritdoc/>
    public async Task<UserStoreResult> GetUserByEmailAsync(string email,
                                                           CancellationToken cancellationToken = default)
    {
        try
        {
            var cacheKey = $"user_email_{email}"; // Cache key based on the email

            // Use FusionCache to manage caching
            var user = await FusionCache.GetOrSetAsync<User>(
                cacheKey,
                async (ctx, ct) =>
                {
                    // Query the UserManager if not found in cache
                    var user = await UserManager.FindByEmailAsync(email);
                    return user!;
                },
                token: cancellationToken
            );

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
    public async Task<UserStoreResult> GetUserByIdAsync(string id,
                                                        CancellationToken cancellationToken = default)
    {
        try
        {
            var cacheKey = $"user_id_{id}"; // Unique cache key based on user ID

            // Use FusionCache to handle caching
            var user = await FusionCache.GetOrSetAsync<User>(cacheKey, async (ctx, token) =>
                {
                    // Query UserManager if user is not found in cache
                    var user = await UserManager.FindByIdAsync(id);
                    return user!;
                }
,               token: cancellationToken);

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
    public async Task<List<string>> GetUserRolesAsync(string email, CancellationToken cancellationToken = default)
    {
            var cacheKey = $"user_roles_{email}"; // Unique cache key for the user's roles

            // Use FusionCache to handle caching
            var roles = await FusionCache.GetOrSetAsync<List<string>>(
                cacheKey,
                async (ctx, token) =>
                {
                    User? user = await UserManager.FindByEmailAsync(email) ?? null!;
                    return [.. await UserManager.GetRolesAsync(user)];
                },
                options: new FusionCacheEntryOptions
                {
                    Duration = TimeSpan.FromMinutes(30), // Cache duration
                    IsFailSafeEnabled = true,            // Enable fail-safe mode
                    FailSafeThrottleDuration = TimeSpan.FromSeconds(30), // Retry interval
                },
                token: cancellationToken
            );

            return roles;
    }

    /// <inheritdoc />
    public async Task<List<User>> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        var cacheKey = "all_users"; // Unique cache key for all users

        // Use FusionCache to handle caching
        var users = await FusionCache.GetOrSetAsync<List<User>>(
            cacheKey,
            async (ctx, token) =>
            {
                List<User> usersInRole = [.. await UserManager.GetUsersInRoleAsync(RoleDefaults.User)];
                return usersInRole;
            },
            token: cancellationToken
        );

        return users;
    }
}
