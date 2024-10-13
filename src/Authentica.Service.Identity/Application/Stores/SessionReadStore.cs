using Application.Contracts;
using Domain.Aggregates.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Stores;
/// <summary>
/// Provides implementation for reading session data from a persistent store.
/// </summary>
public sealed class SessionReadStore : StoreBase, ISessionReadStore
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SessionWriteStore"/> class.
    /// </summary>
    /// <param name="services">The service provider used to resolve dependencies.</param>
    public SessionReadStore(IServiceProvider services) : base(services)
    {
    }
    /// <inheritdoc/>
    public async Task<IList<Session>> GetAsync(string UserId)
    {
        return await DbContext.Sessions.Where(x => x.UserId == UserId).ToListAsync();
    }
    /// <inheritdoc/>
    public async Task<Session> GetByIdAsync(string SessionId)
    {
        return await DbContext.Sessions.Where(x => x.SessionId == SessionId).FirstAsync();
    }
}
