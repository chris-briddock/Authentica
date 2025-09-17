using Application.Constants;
using Domain.Aggregates.Identity;
using Domain.Constants;
using Domain.Contracts.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Stores;
/// <summary>
/// Provides implementation for writing session data to a persistent store.
/// </summary>

public sealed class SessionWriteStore : StoreBase, ISessionWriteStore
{
    private readonly ILogger<SessionWriteStore> _logger;
    private DbSet<Session> DbSet => DbContext.Set<Session>();
    /// <summary>
    /// Initializes a new instance of the <see cref="SessionWriteStore"/> class.
    /// </summary>
    /// <param name="services">The service provider used to resolve dependencies.</param>
    public SessionWriteStore(IServiceProvider services) : base(services)
    {
        _logger = services.GetRequiredService<ILogger<SessionWriteStore>>();
        FusionCache.RemoveByTag(CacheTagConstants.Sessions);
    }

    /// <inheritdoc />
    public async Task<Session> CreateAsync(Session session)
    {
        await DbSet.AddAsync(session);
        await DbContext.SaveChangesAsync();
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting session {SessionId} for user {UserId}", session.SessionId, session.UserId);
            throw;
        }

    }
}
