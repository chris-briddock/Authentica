using Domain.Aggregates.Identity;
using System.Collections.Immutable;

namespace Application.Contracts;

/// <summary>
/// Defines a contract for storing activities.
/// </summary>
public interface IActivityWriteStore
{
    /// <summary>
    /// Saves an activity asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of the activity.</typeparam>
    /// <param name="activity">The activity to save.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SaveActivityAsync<T>(T activity) where T : class;
}