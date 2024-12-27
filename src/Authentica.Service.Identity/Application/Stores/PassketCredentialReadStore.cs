using Application.DTOs;
using Domain.Aggregates.Identity;
using Domain.Contracts.Stores;
using Microsoft.EntityFrameworkCore;

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
           return await DbContext.Set<PasskeyCredential>()
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
    }
}
