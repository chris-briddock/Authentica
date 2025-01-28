using Application.Constants;
using Domain.Aggregates.Identity;
using Domain.Contracts.Stores;
using Fido2NetLib;

namespace Application.Stores;

/// <summary>
/// Represents a store for writing passkey credentials to the database.
/// </summary>
public sealed class PasskeyCredentialWriteStore : StoreBase, IPasskeyCredentialWriteStore
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PasskeyCredentialWriteStore"/> class.
    /// </summary>
    /// <param name="services">The service provider used to resolve dependencies.</param>
    public PasskeyCredentialWriteStore(IServiceProvider services) : base(services)
    {
        FusionCache.RemoveByTag(CacheTagConstants.PasskeyCredentials);
    }

    /// <summary>
    /// Creates a new passkey credential record in the database.
    /// </summary>
    /// <param name="credential">The credential result containing details about the FIDO2 credential to store.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task CreateAsync(Fido2.CredentialMakeResult credential)
    {
        // Add the new credential to the database.
        await DbContext.Set<PasskeyCredential>()
                       .AddAsync(new PasskeyCredential()
                       {
                           CredentialId = credential.Result!.CredentialId, 
                           UserHandle = credential.Result.User.Id,         
                           PublicKey = credential.Result.PublicKey,        
                           SignatureCounter = credential.Result.Counter,   
                           CredType = credential.Result.CredType,          
                           CreatedDate = DateTime.UtcNow,                  
                           AaGuid = credential.Result.Aaguid.ToString()    
                       });

        // Save the changes to the database.
        await DbContext.SaveChangesAsync();
    }
}

