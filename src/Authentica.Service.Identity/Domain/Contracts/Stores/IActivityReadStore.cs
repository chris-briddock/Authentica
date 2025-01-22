using Application.DTOs;

namespace Domain.Contracts.Stores;

/// <summary>
/// Defines a contract for reading activity data.
/// </summary>

public interface IActivityReadStore
{
    /// <summary>
    /// Retrieves a list of activities that occurred at or after the specified timestamp.
    /// </summary>
    /// <param name="timeStamp">The timestamp to filter activities.</param>
    /// <param name="token">Propigates the notification of a cancelled Task</param>
    /// <returns>An list of activities that match the timestamp criteria.</returns>
    Task<List<ActivityDto>> GetActivitiesByDateTimeStampAsync(DateTime timeStamp,
                                                              CancellationToken token = default);
    /// <summary>
    /// Retrieves a list of all activities.
    /// </summary>
    /// <returns>An list of activities.</returns>
    Task<List<ActivityDto>> GetActivitiesAsync(CancellationToken token = default);
}