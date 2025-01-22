using Application.DTOs;

namespace Domain.Contracts.Stores;

/// <summary>
/// Represents a store for reading user's multi factor preferences.
/// </summary>
public interface IUserMultiFactorReadStore
{
    /// <summary>
    /// Retrieves the user's multi factor settings.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="token">Propigates notification that the task should be cancelled.</param>
    /// <returns>The user's multi factor settings.</returns>
    Task<UserMultiFactorReadDto> GetAsync(string userId,
                                          CancellationToken token = default);

    /// <summary>
    /// Checks if email multi factor authentication is enabled for the user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="token">Propigates notification that the task should be cancelled.</param>
    /// <returns>True if email multi factor authentication is enabled, false otherwise.</returns>
    Task<UserMultiFactorReadDto> IsEmailEnabledAsync(string userId,
                                                     CancellationToken token = default);

    /// <summary>
    /// Checks if authenticator multi factor authentication is enabled for the user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="token">Propigates notification that tasks should be cancelled.</param>
    /// <returns>True if authenticator multi factor authentication is enabled, false otherwise.</returns>
    Task<UserMultiFactorReadDto> IsAuthenticatorEnabledAsync(string userId,
                                                             CancellationToken token = default);

    /// <summary>
    /// Checks if passkeys multi factor authentication is enabled for the user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="token">Propigates notification that the task should be cancelled.</param>
    /// <returns>True if passkeys multi factor authentication is enabled, false otherwise.</returns>
    Task<UserMultiFactorReadDto> IsPasskeysEnabledAsync(string userId, CancellationToken token = default);
}
