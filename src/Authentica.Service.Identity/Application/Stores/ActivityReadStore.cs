using Application.Constants;
using Application.DTOs;
using Domain.Aggregates.Identity;
using Domain.Contracts.Stores;
using Microsoft.EntityFrameworkCore;

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

        var activities = await FusionCache.GetOrSetAsync<List<ActivityDto>>(
            cacheKey,
            async (ctx, ct) =>
            {
                ctx.Tags = [CacheTagConstants.Activities];
                // Fetch data from DbSet and project to ActivityDto
                return await DbSet.Select(x => new ActivityDto
                {
                    SequenceId = x.SequenceId,
                    ActivityType = x.ActivityType,
                    CreatedOn = x.CreatedOn,
                    Data = x.Data
                }).ToListAsync(ct);
                
            },
            token: token
        );

        return activities;
    }


    /// <inheritdoc/>
    public async Task<List<ActivityDto>> GetActivitiesByDateTimeStampAsync(DateTime timeStamp, CancellationToken token = default)
    {
        string cacheKey = $"activities_{timeStamp:yyyyMMddHHmmss}";

        // Use FusionCache to manage caching logic
        var activities = await FusionCache.GetOrSetAsync<List<ActivityDto>>(
            cacheKey,
            async (ctx, ct) =>
            {
                ctx.Tags = [CacheTagConstants.Activities];
                // Query the database if the value is not found in the cache
                return await DbSet.Where(x => x.CreatedOn == timeStamp)
                                  .Select(x => new ActivityDto
                                  {
                                      SequenceId = x.SequenceId,
                                      ActivityType = x.ActivityType,
                                      CreatedOn = x.CreatedOn,
                                      Data = x.Data
                                  })
                                  .OrderBy(x => x.CreatedOn)
                                  .ToListAsync(ct);
            },
            token: token
        );

        return activities;
    }
}
