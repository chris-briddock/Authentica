using System.Collections.Immutable;
using Application.Contracts;
using Domain.Aggregates.Identity;

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
    public ActivityReadStore(IServiceProvider services) : base(services) {}
    /// <inheritdoc/>
    public ImmutableList<Activity> GetActivities()
    {
        var activities = DbContext.Activities.ToImmutableList();

        return activities;
    }

    /// <inheritdoc/>
    public ImmutableList<Activity> GetActivitiesByDateTimeStamp(DateTime timeStamp)
    {
        var events = DbContext.Activities.Where(x => x.CreatedOn == timeStamp)
                    .OrderBy(x => x.CreatedOn)
                    .ToImmutableList();

        return events;
    }
}
