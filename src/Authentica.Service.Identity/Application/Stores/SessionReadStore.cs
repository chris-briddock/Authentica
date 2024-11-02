using Application.Contracts;
using Domain.Aggregates.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Stores;
/// <summary>
/// Provides implementation for reading session data from a persistent store.
/// </summary>
public sealed class SessionReadStore : StoreBase, ISessionReadStore
{

    private DbSet<Session> DbSet => DbContext.Set<Session>();
    /// <summary>
    /// Initializes a new instance of the <see cref="SessionWriteStore"/> class.
    /// </summary>
    /// <param name="services">The service provider used to resolve dependencies.</param>
    public SessionReadStore(IServiceProvider services) : base(services)
    {
    }
    /// <inheritdoc/>
    public async Task<List<Session>> GetAsync(string userId)
    {
        var result = await DbSet.Where(x => x.UserId == userId).ToListAsync();
        return result;
    }
    /// <inheritdoc/>
    public async Task<Session?> GetByIdAsync(string sessionId)
    {
        var result = await DbSet.Where(x => x.SessionId == sessionId).FirstAsync();
        return result;
    }
}
