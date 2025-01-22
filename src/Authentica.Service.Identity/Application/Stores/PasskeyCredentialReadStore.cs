using System.Text.Json;
using Application.DTOs;
using Domain.Aggregates.Identity;
using Domain.Contracts.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

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
    public async Task<List<PasskeyCredentialReadDto>> GetPasskeyCredentialsAsync(string UserId, CancellationToken token = default)

    {
         // Check in memory cache
        if (MemoryCache.TryGetValue(UserId, out List<PasskeyCredentialReadDto>? passkeyCredentials))
            return passkeyCredentials!;
        
        // Check redis cache
        if (IsRedisEnabled)
        {
            var redisData = DistributedCache.GetString(UserId);
            if (redisData is not null)
            {
                passkeyCredentials = JsonSerializer.Deserialize<List<PasskeyCredentialReadDto>>(redisData);
                // Store in-memory cache for future requests
                MemoryCache.Set(UserId, passkeyCredentials);
                return passkeyCredentials!;
            }
        }

           var result =  await DbContext.Set<PasskeyCredential>()
                                 .Join(DbContext.Set<UserPasskeyCredential>(),
                                     pc => pc.Id,
                                    upc => upc.PasskeyCredentialId,
                                    (pc, upc) => new { pc, upc })
                                .Where(x => x.upc.UserId == UserId)
                                .Select(x => new PasskeyCredentialReadDto()
                                {
                                    PasskeyCredentialId = x.pc.CredentialId,
                                    SignatureCounter = x.pc.SignatureCounter,
                                    UserHandle = x.pc.UserHandle,
                                    PublicKey = x.pc.PublicKey
                                    
                                })
                                .ToListAsync(token);
            // Cache the result
            MemoryCache.Set(UserId, result);
            if (IsRedisEnabled)
            {
                var redisData = JsonSerializer.Serialize(result);
                await DistributedCache.SetStringAsync(UserId, redisData, token);
            }
            return result;
    }
}
