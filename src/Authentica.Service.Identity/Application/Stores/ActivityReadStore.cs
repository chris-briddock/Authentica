using Application.Constants;
using Application.DTOs;
using Domain.Aggregates.Identity;
using Domain.Contracts.Stores;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Application.Stores;

/// <summary>
/// Provides write operations to the activity log.
/// </summary>
public sealed class ActivityReadStore : StoreBase, IActivityReadStore
{    
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

        // Define the compiled query
        var compiledQuery = EF.CompileAsyncQuery(
            (AppDbContext context) =>
                context.Set<Activity>()
                .AsNoTracking()
                .Select(x => new ActivityDto
                {
                    SequenceId = x.SequenceId,
                    ActivityType = x.ActivityType,
                    CreatedOn = x.CreatedOn,
                    Data = x.Data
                })
                .ToList()
        );

        var activities = await FusionCache.GetOrSetAsync<List<ActivityDto>>(
            cacheKey,
            async (ctx, ct) =>
            {
                ctx.Tags = [CacheTagConstants.Activities];
                // Use compiled query for better performance
                return await compiledQuery(DbContext);
            },
            token: token
        );

        return activities;
    }

    /// <inheritdoc/>
    public async Task<List<ActivityDto>> GetActivitiesByDateTimeStampAsync(DateTime timeStamp, CancellationToken token = default)
    {
        string cacheKey = $"activities_{timeStamp:yyyyMMddHHmmss}";

        // Define the compiled query
        var compiledQuery = EF.CompileAsyncQuery(
            (AppDbContext context, DateTime timestamp) =>
                context.Set<Activity>()
                .AsNoTracking()
                .Where(x => x.CreatedOn == timestamp)
                .Select(x => new ActivityDto
                {
                    SequenceId = x.SequenceId,
                    ActivityType = x.ActivityType,
                    CreatedOn = x.CreatedOn,
                    Data = x.Data
                })
                .OrderBy(x => x.CreatedOn)
                .ToList()
        );

        // Use FusionCache to manage caching logic
        var activities = await FusionCache.GetOrSetAsync<List<ActivityDto>>(
            cacheKey,
            async (ctx, ct) =>
            {
                ctx.Tags = [CacheTagConstants.Activities];
                // Use compiled query for better performance
                return await compiledQuery(DbContext, timeStamp);
            },
            token: token
        );

        return activities;
    }
}
