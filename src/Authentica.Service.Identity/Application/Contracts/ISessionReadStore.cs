using Domain.Aggregates.Identity;

namespace Application.Contracts;

/// <summary>
/// Defines the contract for reading session data from a persistent store.
/// </summary>
public interface ISessionReadStore
{
    /// <summary>
    /// Retrieves a session from the store asynchronously based on the session ID.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the retrieved session, or null if not found.</returns>
    Task<IList<Session>> GetAsync(string UserId);
}
