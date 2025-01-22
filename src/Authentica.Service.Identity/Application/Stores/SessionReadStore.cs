using System.Text.Json;
using Application.DTOs;
using Domain.Aggregates.Identity;
using Domain.Contracts.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

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
    public async Task<List<SessionDto>> GetAsync(string userId,
                                                 CancellationToken cancellation = default)
    {
        // Check in-memory cache
        if (MemoryCache.TryGetValue(userId, out List<SessionDto>? sessions))
            return sessions!;
        
        // Check redis cache
        if (IsRedisEnabled)
        {
            var redisData = DistributedCache.GetString(userId);
            if (redisData is not null)
            {
                sessions = JsonSerializer.Deserialize<List<SessionDto>>(redisData);
                // Store in-memory cache for future requests
                MemoryCache.Set(userId, sessions);
                return sessions!;
            }
        }
        var result = await DbSet.Where(x => x.UserId == userId)
                                .Select(s => new SessionDto
                                {
                                    Status = s.Status,
                                    UserId = s.UserId,
                                    StartDateTime = s.StartDateTime,
                                    EndDateTime = s.EndDateTime,
                                    UserAgent = s.UserAgent,
                                    IpAddress = s.IpAddress,
                                })
                               .ToListAsync();
        // Cache the result
        MemoryCache.Set(userId, result);

        // Cache result in redis
        if (IsRedisEnabled)
        {
            var redisData = JsonSerializer.Serialize(result);
            await DistributedCache.SetStringAsync(userId, redisData, cancellation);
        }
        return result;
    }
    /// <inheritdoc/>
    public async Task<Session> GetByIdAsync(string sessionId,
                                            CancellationToken cancellation = default)
    {
        // Check in memory cache
        if (MemoryCache.TryGetValue(sessionId, out Session? session))
            return session!;
        
        // Check redis cache
        if (IsRedisEnabled)
        {
            var redisData = DistributedCache.GetString(sessionId);
            if (redisData is not null)
            {
                session = JsonSerializer.Deserialize<Session>(redisData);
                // Store in-memory cache for future requests
                MemoryCache.Set(sessionId, session);
                return session!;
            }
        }

        var result = await DbSet.Where(x => x.SessionId == sessionId)
                                .FirstAsync(cancellation);

        // Cache the result
        MemoryCache.Set(sessionId, result);

        // Cache result in redis
        if (IsRedisEnabled)
        {
            var redisData = JsonSerializer.Serialize(result);
            await DistributedCache.SetStringAsync(sessionId, redisData, cancellation);
        }
        
        return result;
    }
}
