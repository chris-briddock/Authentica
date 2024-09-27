using Application.Contracts;
using Domain.Aggregates.Identity;

namespace Application.Stores;
/// <summary>
/// Provides implementation for writing session data to a persistent store.
/// </summary>

public sealed class SessionWriteStore : StoreBase, ISessionWriteStore
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SessionWriteStore"/> class.
    /// </summary>
    /// <param name="services">The service provider used to resolve dependencies.</param>
    public SessionWriteStore(IServiceProvider services) : base(services)
    {
    }

    /// <summary>
    /// Creates a new session in the store asynchronously.
    /// </summary>
    /// <param name="session">The session to create.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created session.</returns>
    public async Task<Session> CreateAsync(Session session)
    {
        await DbContext.Sessions.AddAsync(session);
        await DbContext.SaveChangesAsync();
        return session;
    }

    /// <summary>
    /// Deletes a session from the store asynchronously.
    /// </summary>
    /// <param name="session">The session to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task DeleteAsync(Session session)
    {
        DbContext.Sessions.Remove(session);
        await DbContext.SaveChangesAsync();
    }
}
