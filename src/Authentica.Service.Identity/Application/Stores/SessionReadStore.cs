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
    /// <summary>
    /// Retrieves a session from the store asynchronously based on the user ID.
    /// </summary>
    /// <param name="UserId">The ID of the user whose session is to be retrieved.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of sessions associated with the user.</returns>
    public async Task<IList<Session>> GetAsync(string UserId)
    {
        return await DbContext.Sessions.Where(x => x.UserId == UserId).ToListAsync();
    }
}
