using Application.Constants;
using Application.DTOs;
using Domain.Aggregates.Identity;
using Domain.Contracts.Stores;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Application.Stores;

/// <summary>
/// Handles read operations related to passkey credentials.
/// </summary>
public sealed class PasskeyCredentialReadStore : StoreBase, IPasskeyCredentialReadStore
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
        var cacheKey = userId;

        // Define the compiled query
        var compiledQuery = EF.CompileAsyncQuery(
            (AppDbContext context, string uId) =>
                context.Set<PasskeyCredential>()
                    .Join(context.Set<UserPasskeyCredential>(),
                        pc => pc.Id,
                        upc => upc.PasskeyCredentialId,
                        (pc, upc) => new { pc, upc })
                    .Where(x => x.upc.UserId == uId)
                    .Select(x => new PasskeyCredentialReadDto
                    {
                        PasskeyCredentialId = x.pc.CredentialId,
                        SignatureCounter = x.pc.SignatureCounter,
                        UserHandle = x.pc.UserHandle,
                        PublicKey = x.pc.PublicKey
                    })
                    .ToList()
        );

        var result = await FusionCache.GetOrSetAsync<List<PasskeyCredentialReadDto>>(
            cacheKey,
            async (ctx, ct) =>
            {
                ctx.Tags = [CacheTagConstants.PasskeyCredentials];
                // Execute the compiled query
                return await compiledQuery(DbContext, userId);
            },
            token: token
        );

        return result;
    }
}
