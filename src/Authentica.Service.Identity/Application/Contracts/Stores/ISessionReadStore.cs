using Domain.Aggregates.Identity;

namespace Application.Contracts;

/// <summary>
/// Defines the contract for reading session data from a persistent store.
/// </summary>
public interface ISessionReadStore
{
    /// <summary>
    /// Retrieves all sessions from the store asynchronously based on the user id.
    /// </summary>
    /// <param name="userId">The user's unique identifier.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the retrieved session, or null if not found.</returns>
    Task<List<Session>> GetAsync(string userId);
    /// <summary>
    /// Retrieves a session from the store based on the session id.
    /// </summary>
    /// <param name="sessionId">The session unique identifier.</param>
    /// <returns>A task that represents the asynchronous operation. </returns>
    Task<Session?> GetByIdAsync(string sessionId);
}
