using System.Text.Json;
using Application.Constants;
using Application.DTOs;
using Domain.Aggregates.Identity;
using Domain.Contracts.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using ZiggyCreatures.Caching.Fusion;

namespace Application.Stores;

/// <summary>
/// Handles read operations related to passkey credentials.
/// </summary>
public class PasskeyCredentialReadStore : StoreBase, IPasskeyCredentialReadStore
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PasskeyCredentialReadStore"/>
    /// </summary>
    /// <param name="services">The service provider to use for retrieving services.</param>
    public PasskeyCredentialReadStore(IServiceProvider services) : base(services)
    {

    }
    /// <inheritdoc/>
    public async Task<List<PasskeyCredentialReadDto>> GetPasskeyCredentialsAsync(string userId, CancellationToken token = default)
    {
        // Use FusionCache to manage caching
        var cacheKey = userId;  // Cache key based on the UserId

        var result = await FusionCache.GetOrSetAsync<List<PasskeyCredentialReadDto>>(
            cacheKey,
            async (ctx, ct) =>
            {
                ctx.Tags = [CacheTagConstants.PasskeyCredentials];
                // Query the database if the value is not found in the cache
                return await DbContext.Set<PasskeyCredential>()
                    .Join(DbContext.Set<UserPasskeyCredential>(),
                        pc => pc.Id,
                        upc => upc.PasskeyCredentialId,
                        (pc, upc) => new { pc, upc })
                    .Where(x => x.upc.UserId == userId)
                    .Select(x => new PasskeyCredentialReadDto
                    {
                        PasskeyCredentialId = x.pc.CredentialId,
                        SignatureCounter = x.pc.SignatureCounter,
                        UserHandle = x.pc.UserHandle,
                        PublicKey = x.pc.PublicKey
                    })
                    .ToListAsync(ct);
            },
            token: token
        );

        return result;
    }
}
