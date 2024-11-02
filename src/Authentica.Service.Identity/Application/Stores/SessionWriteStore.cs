using Application.Contracts;
using Domain.Aggregates.Identity;
using Domain.Constants;
using Microsoft.EntityFrameworkCore;

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
        try
        {
            var dbSet = DbContext.Set<Session>();
            await dbSet.AddAsync(session);
            await DbContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }

        return session;
    }

    /// <summary>
    /// Soft deletes a session from the store asynchronously.
    /// </summary>
    /// <param name="session">The session to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task DeleteAsync(Session session)
    {
        try
        {
            var dbSet = DbContext.Set<Session>();

            var entry = dbSet.Where(x => x.SessionId == session.SessionId);

            await entry.ExecuteUpdateAsync(x => x
                .SetProperty(s => s.EntityDeletionStatus.IsDeleted, true)
                .SetProperty(s => s.EndDateTime, DateTime.UtcNow)
                .SetProperty(s => s.EntityDeletionStatus.DeletedBy, session.UserId)
                .SetProperty(s => s.EntityDeletionStatus.DeletedOnUtc, DateTime.UtcNow)
                .SetProperty(s => s.Status, SessionStatus.Terminated));
        }
        catch (Exception)
        {
            throw;
        }

    }
}
