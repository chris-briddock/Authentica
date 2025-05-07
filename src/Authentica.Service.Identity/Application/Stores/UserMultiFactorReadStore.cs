using Application.Constants;
using Application.DTOs;
using Domain.Aggregates.Identity;
using Domain.Contracts.Stores;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using ZiggyCreatures.Caching.Fusion;

namespace Application.Stores;

/// <summary>
/// Represents a store for user's multi factor preferences.
/// </summary>
public sealed class UserMultiFactorReadStore : StoreBase, IUserMultiFactorReadStore
{
    private DbSet<UserMultiFactorSettings> DbSet => DbContext.Set<UserMultiFactorSettings>();
    /// <summary>
    /// Initializes a new instance of the <see cref="UserMultiFactorReadStore"/>
    /// </summary>
    /// <param name="services">The service provider used to resolve dependencies.</param>
    public UserMultiFactorReadStore(IServiceProvider services) : base(services)
    {
    }

    /// <inheritdoc/>
    public async Task<UserMultiFactorReadDto> GetAsync(string userId,
                                                       CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(userId);
        
        var cacheKey = userId;

        // Define the compiled query
        var compiledQuery = EF.CompileAsyncQuery(
            (AppDbContext context, string uId) =>
                context.Set<UserMultiFactorSettings>()
                    .Where(x => x.UserId == uId)
                    .Select(x => new UserMultiFactorReadDto
                    {
                        MultiFactorEmailEnabled = x.MultiFactorEmailEnabled,
                        MultiFactorAuthenticatorEnabled = x.MultiFactorAuthenticatorEnabled,
                        MultiFactorPasskeysEnabled = x.MultiFactorPasskeysEnabled
                    })
                    .Single()
        );

        var result = await FusionCache.GetOrSetAsync<UserMultiFactorReadDto>(
            cacheKey,
            async (ctx, ct) =>
            {
                ctx.Tags = [CacheTagConstants.MultiFactorSettings];
                // Execute the compiled query
                return await compiledQuery(DbContext, userId);
            },
            token: token
        );

        return result!;
    }

    /// <inheritdoc/>
    public async Task<UserMultiFactorReadDto> IsEmailEnabledAsync(string userId, CancellationToken token = default)
    {
        // Define the compiled query
        var compiledQuery = EF.CompileAsyncQuery(
            (AppDbContext context, string uId) =>
                context.Set<UserMultiFactorSettings>()
                    .Where(x => x.UserId == uId)
                    .Select(x => new UserMultiFactorReadDto
                    {
                        MultiFactorEmailEnabled = x.MultiFactorEmailEnabled
                    })
                    .Single()
        );

        // Use FusionCache to manage caching
        var cacheKey = userId;  // Cache key based on the userId

        var result = await FusionCache.GetOrSetAsync<UserMultiFactorReadDto>(
            cacheKey,
            async (ctx, ct) =>
            {
                // Execute the compiled query
                return await compiledQuery(DbContext, userId);
            },
            options: new FusionCacheEntryOptions
            {
                Duration = TimeSpan.FromMinutes(10), // Cache duration
                IsFailSafeEnabled = true,            // Enable fail-safe mode
                FailSafeThrottleDuration = TimeSpan.FromSeconds(30), // Retry interval
            },
            token: token
        );

        return result;
    }

    /// <inheritdoc/>
    public async Task<UserMultiFactorReadDto> IsAuthenticatorEnabledAsync(string userId, CancellationToken token)
    {
        // Define the compiled query
        var compiledQuery = EF.CompileAsyncQuery(
            (AppDbContext context, string uId) =>
                context.Set<UserMultiFactorSettings>()
                    .Where(x => x.UserId == uId)
                    .Select(x => new UserMultiFactorReadDto
                    {
                        MultiFactorAuthenticatorEnabled = x.MultiFactorAuthenticatorEnabled
                    })
                    .Single()
        );

        // Use FusionCache to manage caching
        var cacheKey = userId;  // Cache key based on the userId

        var result = await FusionCache.GetOrSetAsync<UserMultiFactorReadDto>(
            cacheKey,
            async (ctx, ct) =>
            {
                // Execute the compiled query
                return await compiledQuery(DbContext, userId);
            },
            token: token
        );

        return result;
    }

    /// <inheritdoc/>
    public async Task<UserMultiFactorReadDto> IsPasskeysEnabledAsync(string userId, CancellationToken token)
    {
        // Define the compiled query
        var compiledQuery = EF.CompileAsyncQuery(
            (AppDbContext context, string uId) =>
                context.Set<UserMultiFactorSettings>()
                    .Where(x => x.UserId == uId)
                    .Select(x => new UserMultiFactorReadDto
                    {
                        MultiFactorPasskeysEnabled = x.MultiFactorPasskeysEnabled
                    })
                    .Single()
        );

        // Use FusionCache to manage caching
        var cacheKey = userId;  // Cache key based on the userId

        var result = await FusionCache.GetOrSetAsync<UserMultiFactorReadDto>(
            cacheKey,
            async (ctx, ct) =>
            {
                // Execute the compiled query
                return await compiledQuery(DbContext, userId);
            },
            token: token
        );

        return result!;
    }
}
