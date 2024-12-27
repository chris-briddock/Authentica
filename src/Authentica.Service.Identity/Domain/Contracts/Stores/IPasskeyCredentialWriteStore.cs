using Fido2NetLib;

namespace Domain.Contracts.Stores;

/// <summary>
/// Defines a contract for storing passkey credentials.
/// </summary>
public interface IPasskeyCredentialWriteStore
{
    /// <summary>
    /// Asynchronously creates a new passkey credential.
    /// </summary>
    /// <param name="credential">The credential to be created.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task CreateAsync(Fido2.CredentialMakeResult credential);
}
