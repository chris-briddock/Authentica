using Application.DTOs;

namespace Domain.Contracts.Stores;

/// <summary>
/// Defines a contract for reading passkey credentials from a store.
/// </summary>
public interface IPasskeyCredentialReadStore
{
    /// <summary>
    /// Retrieves a list of passkey credentials associated with the specified user ID.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve passkey credentials for.</param>
    /// <param name="token">The cancellation token to cancel the operation.</param>
    /// <returns>A list of <see cref="PasskeyCredentialReadDto"/> objects representing the passkey credentials.</returns>
    public Task<List<PasskeyCredentialReadDto>> GetPasskeyCredentialsAsync(string userId, CancellationToken token = default);
}
