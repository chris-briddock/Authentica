using Application.Constants;
using Domain.Aggregates.Identity;
using Domain.Constants;
using Domain.Contracts.Stores;
using Microsoft.EntityFrameworkCore;

namespace Application.Stores;
/// <summary>
/// Provides implementation for writing session data to a persistent store.
/// </summary>

public sealed class SessionWriteStore : StoreBase, ISessionWriteStore
{
    private DbSet<Session> DbSet => DbContext.Set<Session>();
    /// <summary>
    /// Initializes a new instance of the <see cref="SessionWriteStore"/> class.
    /// </summary>
    /// <param name="services">The service provider used to resolve dependencies.</param>
    public SessionWriteStore(IServiceProvider services) : base(services)
    {
        FusionCache.RemoveByTag(CacheTagConstants.Sessions);
    }

    /// <inheritdoc />
    public async Task<Session> CreateAsync(Session session)
    {
        var strategy = DbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            try
            {
                await DbContext.Sessions.AddAsync(session);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                // Re-fetch to confirm if the session already exists
                var existingSession = await DbContext.Sessions
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.SessionId == session.SessionId);

                if (existingSession is not null)
                    return;

                // Retry only if session truly doesn't exist
                await DbContext.Sessions.AddAsync(session);
                await DbContext.SaveChangesAsync();
            }
        });

        return session;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Session session)
    {
        try
        {

            var entry = DbSet.Where(x => x.SessionId == session.SessionId);

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
