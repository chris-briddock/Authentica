using System.Text.Json;
using Application.Constants;
using Application.DTOs;
using Domain.Aggregates.Identity;
using Domain.Contracts.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
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
    public async Task<UserMultiFactorReadDto> GetAsync(string userId, CancellationToken token = default)
    {
        // Use FusionCache to manage caching
        var cacheKey = userId;  // Cache key based on the userId

        var result = await FusionCache.GetOrSetAsync<UserMultiFactorReadDto>(
            cacheKey,
            async (ctx, ct) =>
            {
                ctx.Tags = [CacheTagConstants.MultiFactorSettings];
                // Query the database if the value is not found in the cache
                return await DbSet
                    .Where(x => x.UserId == userId)
                    .Select(x => new UserMultiFactorReadDto
                    {
                        MultiFactorEmailEnabled = x.MultiFactorEmailEnabled,
                        MultiFactorAuthenticatorEnabled = x.MultiFactorAuthenticatorEnabled,
                        MultiFactorPasskeysEnabled = x.MultiFactorPasskeysEnabled
                    })
                    .SingleAsync(ct);
            },
            token: token
        );

        return result!;
    }

    /// <inheritdoc/>
    public async Task<UserMultiFactorReadDto> IsEmailEnabledAsync(string userId, CancellationToken token = default)
    {
        // Use FusionCache to manage caching
        var cacheKey = userId;  // Cache key based on the userId

        var result = await FusionCache.GetOrSetAsync<UserMultiFactorReadDto>(
            cacheKey,
            async (ctx, ct) =>
            {
                // Query the database if the value is not found in the cache
                return await DbSet
                    .Where(x => x.UserId == userId)
                    .Select(x => new UserMultiFactorReadDto
                    {
                        MultiFactorEmailEnabled = x.MultiFactorEmailEnabled
                    })
                    .SingleAsync(ct);
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
        // Use FusionCache to manage caching
        var cacheKey = userId;  // Cache key based on the userId

        var result = await FusionCache.GetOrSetAsync<UserMultiFactorReadDto>(
            cacheKey,
            async (ctx, ct) =>
            {
                // Query the database if the value is not found in the cache
                return await DbSet
                    .Where(x => x.UserId == userId)
                    .Select(x => new UserMultiFactorReadDto
                    {
                        MultiFactorAuthenticatorEnabled = x.MultiFactorAuthenticatorEnabled
                    })
                    .SingleAsync(ct);
            },
            token: token
        );

        return result;
    }

    /// <inheritdoc/>
    public async Task<UserMultiFactorReadDto> IsPasskeysEnabledAsync(string userId, CancellationToken token)
    {
        // Use FusionCache to manage caching
        var cacheKey = userId;  // Cache key based on the userId

        var result = await FusionCache.GetOrSetAsync<UserMultiFactorReadDto>(
            cacheKey,
            async (ctx, ct) =>
            {
                // Query the database if the value is not found in the cache
                return await DbSet
                    .Where(x => x.UserId == userId)
                    .Select(x => new UserMultiFactorReadDto
                    {
                        MultiFactorPasskeysEnabled = x.MultiFactorPasskeysEnabled
                    })
                    .SingleAsync(ct);
            },
            token: token
        );

        return result!;
    }
}
