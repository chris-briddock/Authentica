using Application.DTOs;
using Domain.Aggregates.Identity;
using Domain.Contracts.Stores;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace Application.Stores;

/// <summary>
/// Provides write operations to the event log.
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
    public ActivityReadStore(IServiceProvider services) : base(services) { }
    /// <inheritdoc/>
    public List<ActivityDto> GetActivities()
    {
        var activities = DbSet.Select(x => new ActivityDto
        {
            SequenceId = x.SequenceId,
            ActivityType = x.ActivityType,
            CreatedOn = x.CreatedOn,
            Data = x.Data
        })
        .ToList();

        return activities;
    }

    /// <inheritdoc/>
    public List<ActivityDto> GetActivitiesByDateTimeStamp(DateTime timeStamp)
    {
        var events = DbSet.Where(x => x.CreatedOn == timeStamp)
                                         .Select(x => new ActivityDto
                                         {
                                             SequenceId = x.SequenceId,
                                             ActivityType = x.ActivityType,
                                             CreatedOn = x.CreatedOn,
                                             Data = x.Data
                                         })
                                         .OrderBy(x => x.CreatedOn)
                                         .ToList();

        return events;
    }
}
