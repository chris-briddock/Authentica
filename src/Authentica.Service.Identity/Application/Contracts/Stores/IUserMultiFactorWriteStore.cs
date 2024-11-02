using Application.Results;

namespace Application.Contracts;

/// <summary>
/// Defines the operations for managing multi-factor authentication (MFA) settings for a user.
/// </summary>
public interface IUserMultiFactorWriteStore
{
    /// <summary>
    /// Enables or disables the authenticator-based multi-factor authentication for the specified user.
    /// </summary>
    /// <param name="isEnabled">A flag indicating whether the authenticator MFA should be enabled.</param>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A task that represents the asynchronous operation, containing the result of the MFA operation.</returns>
    Task<UserMultiFactorResult> SetAutheticatorAsync(bool isEnabled, string userId);

    /// <summary>
    /// Enables or disables email-based multi-factor authentication for the specified user.
    /// </summary>
    /// <param name="isEnabled">A flag indicating whether the email MFA should be enabled.</param>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A task that represents the asynchronous operation, containing the result of the MFA operation.</returns>
    Task<UserMultiFactorResult> SetEmailAsync(bool isEnabled, string userId);

    /// <summary>
    /// Enables or disables passkey-based multi-factor authentication for the specified user.
    /// </summary>
    /// <param name="isEnabled">A flag indicating whether passkey MFA should be enabled.</param>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A task that represents the asynchronous operation, containing the result of the MFA operation.</returns>
    Task<UserMultiFactorResult> SetPasskeysAsync(bool isEnabled, string userId);
}