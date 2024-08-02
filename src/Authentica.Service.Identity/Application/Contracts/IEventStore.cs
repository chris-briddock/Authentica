using Domain.Aggregates.Identity;
using System.Collections.Immutable;

namespace Application.Contracts;

/// <summary>
/// Defines a contract for storing and retrieving events.
/// </summary>
public interface IEventStore
{
    /// <summary>
    /// Saves an event asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of the event.</typeparam>
    /// <param name="event">The event to save.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SaveEventAsync<T>(T @event) where T : class;

    /// <summary>
    /// Retrieves a list of events that occurred at or after the specified timestamp.
    /// </summary>
    /// <param name="timeStamp">The timestamp to filter events.</param>
    /// <returns>An immutable list of events that match the timestamp criteria.</returns>
    ImmutableList<Event> GetEventsByTimeStamp(DateTime timeStamp);

    /// <summary>
    /// Retrieves a list of events associated with the specified session ID.
    /// </summary>
    /// <param name="sessionId">The session ID to filter events.</param>
    /// <returns>An immutable list of events that match the session ID criteria.</returns>
    ImmutableList<Event> GetEventsBySessionId(string sessionId);
}
