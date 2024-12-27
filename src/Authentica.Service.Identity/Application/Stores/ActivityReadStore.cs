using Application.DTOs;
using Domain.Contracts.Stores;
using System.Collections.Immutable;

namespace Application.Stores;

/// <summary>
/// Provides write operations to the event log.
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
    public ActivityReadStore(IServiceProvider services) : base(services) { }
    /// <inheritdoc/>
    public ImmutableList<ActivityDto> GetActivities()
    {
        var activities = DbContext.Activities
                                  .Select(x => new ActivityDto
                                  {
                                      SequenceId = x.SequenceId,
                                      ActivityType = x.ActivityType,
                                      CreatedOn = x.CreatedOn,
                                      Data = x.Data
                                  })
                                   .ToImmutableList();

        return activities;
    }

    /// <inheritdoc/>
    public ImmutableList<ActivityDto> GetActivitiesByDateTimeStamp(DateTime timeStamp)
    {
        var events = DbContext.Activities.Where(x => x.CreatedOn == timeStamp)
                                         .Select(x => new ActivityDto
                                         {
                                             SequenceId = x.SequenceId,
                                             ActivityType = x.ActivityType,
                                             CreatedOn = x.CreatedOn,
                                             Data = x.Data
                                         })
                                         .OrderBy(x => x.CreatedOn)
                                         .ToImmutableList();

        return events;
    }
}
