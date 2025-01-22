using System.Text.Json;
using Application.DTOs;
using Domain.Aggregates.Identity;
using Domain.Contracts.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

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
            // Check in memory cache
            if (MemoryCache.TryGetValue(userId, out UserMultiFactorReadDto? settings))
                return settings!;

            settings = await DbSet
                .Where(x => x.UserId == userId)
                .Select(x => new UserMultiFactorReadDto
                {
                    MultiFactorEmailEnabled = x.MultiFactorEmailEnabled,
                    MultiFactorAuthenticatorEnabled = x.MultiFactorAuthenticatorEnabled,
                    MultiFactorPasskeysEnabled = x.MultiFactorPasskeysEnabled
                })
                .FirstOrDefaultAsync(token);
            // Store the result in memory cache
            MemoryCache.Set(userId, settings);
            // Store the result in redis cache
            if (IsRedisEnabled)
                await DistributedCache.SetStringAsync(userId, JsonSerializer.Serialize(settings), token);

        return settings!;
    }

    /// <inheritdoc/>
    public async Task<UserMultiFactorReadDto> IsEmailEnabledAsync(string userId, CancellationToken token = default)
    {
        // check in memory cache
        if (MemoryCache.TryGetValue(userId, out UserMultiFactorReadDto? settings))
            return settings!;

        // check in redis cache
        if (IsRedisEnabled)
        {
            var cacheValue = await DistributedCache.GetStringAsync(userId, token);
            if (cacheValue is not null)
            {
                settings = JsonSerializer.Deserialize<UserMultiFactorReadDto>(cacheValue);
                return settings!;
            }
        }
        
        settings = await DbSet
                .Where(x => x.UserId == userId)
                .Select(x => new UserMultiFactorReadDto
                {
                    MultiFactorEmailEnabled = x.MultiFactorEmailEnabled
                }).FirstOrDefaultAsync(token);
        // Store the result in memory cache
        MemoryCache.Set(userId, settings);
        // Store the result in redis cache
        if (IsRedisEnabled)
        {
            await DistributedCache.SetStringAsync(userId, JsonSerializer.Serialize(settings), token);
        }

        return settings!;
    }

    /// <inheritdoc/>
    public async Task<UserMultiFactorReadDto> IsAuthenticatorEnabledAsync(string userId, CancellationToken token)
    {
        // check in memory cache
        if (MemoryCache.TryGetValue(userId, out UserMultiFactorReadDto? settings))
            return settings!;

        // check in redis cache
        if (IsRedisEnabled)
        {
            var cacheValue = await DistributedCache.GetStringAsync(userId, token);
            if (cacheValue != null)
            {
                settings = JsonSerializer.Deserialize<UserMultiFactorReadDto>(cacheValue);
                return settings!;
            }
        }

        settings =  await DbSet
                .Where(x => x.UserId == userId)
                .Select(x => new UserMultiFactorReadDto()
                {
                    MultiFactorAuthenticatorEnabled = x.MultiFactorAuthenticatorEnabled
                }).FirstOrDefaultAsync(token);

        // Store the result in memory cache
        MemoryCache.Set(userId, settings);
        
        // store the result in redis cache
        if (IsRedisEnabled)
            await DistributedCache.SetStringAsync(userId, JsonSerializer.Serialize(settings), token);

        return settings!;
    }

    /// <inheritdoc/>
    public async Task<UserMultiFactorReadDto> IsPasskeysEnabledAsync(string userId, CancellationToken token)
    {
        // check in memory cache
        if (MemoryCache.TryGetValue(userId, out UserMultiFactorReadDto? settings))
        {
            return settings!;
        }

        // check in redis cache
        if (IsRedisEnabled)
        {
            var cacheValue = await DistributedCache.GetStringAsync(userId, token);
            if (cacheValue != null)
            {
                settings = JsonSerializer.Deserialize<UserMultiFactorReadDto>(cacheValue);
                return settings!;
            }
        }
        
        settings = await DbSet
                .Where(x => x.UserId == userId)
                .Select(x => new UserMultiFactorReadDto()
                {
                    MultiFactorPasskeysEnabled = x.MultiFactorPasskeysEnabled
                }).FirstOrDefaultAsync(token);

        // Store the result in memory cache
        MemoryCache.Set(userId, settings);

        // Store the result in redis cache
        if (IsRedisEnabled)
        {
            await DistributedCache.SetStringAsync(userId, JsonSerializer.Serialize(settings), token);
        }
        return settings!;

    }
}
