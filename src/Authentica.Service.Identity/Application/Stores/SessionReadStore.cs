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
/// Provides implementation for reading session data from a persistent store.
/// </summary>
public sealed class SessionReadStore : StoreBase, ISessionReadStore
{

    private DbSet<Session> DbSet => DbContext.Set<Session>();
    /// <summary>
    /// Initializes a new instance of the <see cref="SessionWriteStore"/> class.
    /// </summary>
    /// <param name="services">The service provider used to resolve dependencies.</param>
    public SessionReadStore(IServiceProvider services) : base(services)
    {
    }
    /// <inheritdoc/>
    public async Task<List<SessionDto>> GetAsync(string userId, CancellationToken cancellation = default)
    {
        // Use FusionCache to manage caching
        var cacheKey = userId;  // Cache key based on the UserId

        var result = await FusionCache.GetOrSetAsync<List<SessionDto>>(
            cacheKey,
            async (ctx, ct) =>
            {
                ctx.Tags = [CacheTagConstants.Sessions];
                // Query the database if the value is not found in the cache
                return await DbSet
                    .Where(x => x.UserId == userId)
                    .Select(s => new SessionDto
                    {
                        Status = s.Status,
                        UserId = s.UserId,
                        StartDateTime = s.StartDateTime,
                        EndDateTime = s.EndDateTime,
                        UserAgent = s.UserAgent,
                        IpAddress = s.IpAddress
                    })
                    .ToListAsync(ct);
            },
            token: cancellation
        );

        return result;
    }
    /// <inheritdoc/>
    public async Task<Session> GetByIdAsync(string sessionId, CancellationToken cancellation = default)
    {
        // Use FusionCache to manage caching
        var cacheKey = sessionId;  // Cache key based on the sessionId

        var result = await FusionCache.GetOrSetAsync<Session>(
            cacheKey,
            async (ctx, ct) =>
            {
                ctx.Tags = [CacheTagConstants.Sessions];
                // Query the database if the value is not found in the cache
                return await DbSet
                    .Where(x => x.SessionId == sessionId)
                    .FirstAsync(ct);
            },
            options: new FusionCacheEntryOptions
            {
                Duration = TimeSpan.FromMinutes(10), // Cache duration
                IsFailSafeEnabled = true,            // Enable fail-safe mode
                FailSafeThrottleDuration = TimeSpan.FromSeconds(30), // Retry interval
            },
            token: cancellation
        );

        return result;
    }

}
