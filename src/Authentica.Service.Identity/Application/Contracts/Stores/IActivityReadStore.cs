using Domain.Aggregates.Identity;
using System.Collections.Immutable;

namespace Application.Contracts;

/// <summary>
/// Defines a contract for reading activity data.
/// </summary>

public interface IActivityReadStore
{
    /// <summary>
    /// Retrieves a list of activities that occurred at or after the specified timestamp.
    /// </summary>
    /// <param name="timeStamp">The timestamp to filter activities.</param>
    /// <returns>An immutable list of activities that match the timestamp criteria.</returns>
    ImmutableList<Activity> GetActivitiesByDateTimeStamp(DateTime timeStamp);
    /// <summary>
    /// Retrieves a list of all activities.
    /// </summary>
    /// <returns>An immutable list of activities.</returns>
    ImmutableList<Activity> GetActivities();
}