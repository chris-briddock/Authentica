using Application.Constants;
using Application.DTOs;
using Domain.Aggregates.Identity;
using Domain.Contracts.Stores;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

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
    public async Task<List<SessionDto>> GetAsync(string userId, CancellationToken cancellation = default)
    {
        var cacheKey = userId;

        // Define the compiled query
        var compiledQuery = EF.CompileAsyncQuery(
            (AppDbContext context, string uId) =>
                context.Set<Session>()
                    .Where(x => x.UserId == uId)
                    .Select(s => new SessionDto
                    {
                        Status = s.Status,
                        UserId = s.UserId,
                        StartDateTime = s.StartDateTime,
                        EndDateTime = s.EndDateTime,
                        UserAgent = s.UserAgent,
                        IpAddress = s.IpAddress
                    })
                    .ToList()
        );

        var result = await FusionCache.GetOrSetAsync<List<SessionDto>>(
            cacheKey,
            async (ctx, ct) =>
            {
                ctx.Tags = [CacheTagConstants.Sessions];
                // Execute the compiled query
                return await compiledQuery(DbContext, userId);
            },
            token: cancellation
        );

        return result;
    }
    /// <inheritdoc/>
    public async Task<Session?> GetByIdAsync(string sessionId, CancellationToken cancellation = default)
    {
        // Use FusionCache to manage caching
        var cacheKey = sessionId;  // Cache key based on the sessionId

        // Define the compiled query
        var compiledQuery = EF.CompileAsyncQuery(
            (AppDbContext context, string sId) =>
                context.Set<Session>()
                    .Where(x => x.SessionId == sId)
                    .FirstOrDefault()
        );

        var result = await FusionCache.GetOrSetAsync<Session>(
            cacheKey,
            async (ctx, ct) =>
            {
                ctx.Tags = [CacheTagConstants.Sessions];
                // Execute the compiled query
                var query = await compiledQuery(DbContext, sessionId);
                return query!;
            },
            token: cancellation
        );

        return result;
    }

}
