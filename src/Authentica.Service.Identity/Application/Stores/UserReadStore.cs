using Api.Constants;
using Application.Constants;
using Application.Factories;
using Application.Results;
using Domain.Aggregates.Identity;
using Domain.Contracts.Stores;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
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
    public async Task<UserStoreResult> GetUserByEmailAsync(ClaimsPrincipal claimsPrincipal,
                                                           CancellationToken cancellationToken = default)
    {
        var emailClaim = claimsPrincipal.FindFirst(ClaimTypes.Email);

        if (emailClaim is null)
            return UserStoreResult.Failed(IdentityErrorFactory.EmailNotFound());

        var email = emailClaim.Value;

        var cacheKey = $"user_email_{email}";

        // Define the compiled query
        var compiledQuery = EF.CompileAsyncQuery(
            (AppDbContext context, string emailAddress) =>
                context.Users
                    .AsNoTracking()
                    .FirstOrDefault(u => u.Email == emailAddress)
        );

        var user = await FusionCache.GetOrSetAsync<User>(
            cacheKey,
            async (ctx, ct) =>
            {
                ctx.Tags = [CacheTagConstants.Users];
                // Execute the compiled query
                var result = await compiledQuery(DbContext, email);
                return result!;
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
            var cacheKey = $"user_email_{email}";

            // Define the compiled query
            var compiledQuery = EF.CompileAsyncQuery(
                (AppDbContext context, string emailAddress) =>
                    context.Users
                        .AsNoTracking()
                        .FirstOrDefault(u => u.Email == emailAddress)
            );

            var user = await FusionCache.GetOrSetAsync<User>(
                cacheKey,
                async (ctx, ct) =>
                {
                    ctx.Tags = [CacheTagConstants.Users];
                    // Execute the compiled query
                    var result = await compiledQuery(DbContext, email);
                    return result!;
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
            var cacheKey = $"user_id_{id}";

            // Define the compiled query
            var compiledQuery = EF.CompileAsyncQuery(
                (AppDbContext context, string userId) =>
                    context.Users
                        .AsNoTracking()
                        .FirstOrDefault(u => u.Id == userId)
            );

            var user = await FusionCache.GetOrSetAsync<User>(
                cacheKey,
                async (ctx, ct) =>
                {
                    ctx.Tags = [CacheTagConstants.Users];
                    // Execute the compiled query
                    var result = await compiledQuery(DbContext, id);
                    return result!;
                },
                token: cancellationToken
            );

            if (user is null)
                return UserStoreResult.Failed(IdentityErrorFactory.UserNotFound());

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
        var cacheKey = $"user_roles_{email}";

        var roles = await FusionCache.GetOrSetAsync<List<string>>(
            cacheKey,
            async (ctx, token) =>
            {
                ctx.Tags = [CacheTagConstants.Users];
                // First find the user by email
                User? user = await UserManager.FindByEmailAsync(email) ?? null!;
                // Then get the roles for that user
                return [.. await UserManager.GetRolesAsync(user)];
            },
            token: cancellationToken
        );

        return roles;
    }

    /// <inheritdoc />
    public async Task<List<User>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        var cacheKey = "all_users";

        // Define the compiled query for getting all users
        var compiledQuery = EF.CompileAsyncQuery(
            (AppDbContext context) =>
                context.Users
                    .AsNoTracking()
                    .ToList()
        );

        var users = await FusionCache.GetOrSetAsync<List<User>>(
            cacheKey,
            async (ctx, token) =>
            {
                ctx.Tags = [CacheTagConstants.Users];
                // For getting users in a specific role, we still need to use UserManager
                // as this is specialized functionality provided by Identity
                List<User> usersInRole = [.. await UserManager.GetUsersInRoleAsync(RoleDefaults.User)];
                return usersInRole;
            },
            token: cancellationToken
        );

        return users;
    }
}
