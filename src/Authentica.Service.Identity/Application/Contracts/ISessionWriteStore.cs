using Domain.Aggregates.Identity;

namespace Application.Contracts;

/// <summary>
/// Defines the contract for writing session data to a persistent store.
/// </summary>
public interface ISessionWriteStore
{
    /// <summary>
    /// Creates a new session in the store asynchronously.
    /// </summary>
    /// <param name="session">The session to create.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created session.</returns>
    Task<Session> CreateAsync(Session session);

    /// <summary>
    /// Deletes a session from the store asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteAsync(Session session);
}