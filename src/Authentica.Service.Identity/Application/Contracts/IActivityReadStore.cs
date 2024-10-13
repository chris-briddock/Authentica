using System.Collections.Immutable;
using Domain.Aggregates.Identity;

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
    /// <returns>An immutable list of events that match the timestamp criteria.</returns>
    ImmutableList<Activity> GetActivitiesByTimeStamp(DateTime timeStamp);

    /// <summary>
    /// Retrieves a list of activities associated with the specified session ID.
    /// </summary>
    /// <param name="sessionId">The session ID to filter activities.</param>
    /// <returns>An immutable list of activities that match the session ID criteria.</returns>
    ImmutableList<Activity> GetActivitiesBySessionId(string sessionId);
}