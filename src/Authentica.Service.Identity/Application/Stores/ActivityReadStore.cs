using Application.DTOs;
using Domain.Aggregates.Identity;
using Domain.Contracts.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace Application.Stores;

/// <summary>
/// Provides write operations to the activity log.
/// </summary>
public sealed class ActivityReadStore : StoreBase, IActivityReadStore
{
    
    /// <summary>
    /// Gets the Activity DbSet.
    /// </summary>
    private DbSet<Activity> DbSet => DbContext.Set<Activity>();
    /// <summary>
    /// Initializes a new instance of the <see cref="ActivityReadStore"/> class.
    /// </summary>
    /// <param name="services">The service provider to retrieve required services for the write store operations.</param>
    /// <remarks>
    /// This constructor initializes the <see cref="ActivityReadStore"/> instance by calling the base constructor with the provided service provider.
    /// </remarks>
    public ActivityReadStore(IServiceProvider services) : base(services) {}
    /// <inheritdoc/>
    public async Task<List<ActivityDto>> GetActivitiesAsync(CancellationToken token = default)
    { 
        const string cacheKey = "activities";

        // Check in-memory cache
        if (MemoryCache.TryGetValue(cacheKey, out List<ActivityDto>? activities))
            return activities!;

        // Check Redis cache
        if (IsRedisEnabled)
        {
            var redisData = DistributedCache.GetString(cacheKey);

            if (redisData is not null)
            {
                activities = JsonSerializer.Deserialize<List<ActivityDto>>(redisData);
                // Store in-memory cache for future requests
                MemoryCache.Set(cacheKey, activities);
                return activities!;
            }
        }

        var result = await DbSet.Select(x => new ActivityDto
        {
            SequenceId = x.SequenceId,
            ActivityType = x.ActivityType,
            CreatedOn = x.CreatedOn,
            Data = x.Data
        }).ToListAsync(token);

        MemoryCache.Set(cacheKey, activities);
        
        if (IsRedisEnabled)
            await DistributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(activities), token);

        return result;
    }

    /// <inheritdoc/>
    public async Task<List<ActivityDto>> GetActivitiesByDateTimeStampAsync(DateTime timeStamp,
                                                                           CancellationToken token = default)
    {
        string cacheKey = $"activities_{timeStamp:yyyyMMddHHmmss}";

        // Check in-memory cache.
        if (MemoryCache.TryGetValue(cacheKey, out List<ActivityDto>? events))
            return await Task.FromResult(events!);

        // Check redis cache if enabled.
        if (IsRedisEnabled)
        {
            var redisData = DistributedCache.GetString(cacheKey);

            if (redisData is not null)
            {
                events = JsonSerializer.Deserialize<List<ActivityDto>>(redisData);
                MemoryCache.Set(cacheKey, events);
                return await Task.FromResult(events!);
            }
        }

        // Query the database.
        var result = await DbSet.Where(x => x.CreatedOn == timeStamp)
                                         .Select(x => new ActivityDto
                                         {
                                             SequenceId = x.SequenceId,
                                             ActivityType = x.ActivityType,
                                             CreatedOn = x.CreatedOn,
                                             Data = x.Data
                                         })
                                         .OrderBy(x => x.CreatedOn)
                                         .ToListAsync(token);
        
        MemoryCache.Set(cacheKey, events);
        
        if (IsRedisEnabled)
            await DistributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(events), token);

        return result;
    }
}
