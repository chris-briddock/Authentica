using Fido2NetLib;
using Fido2NetLib.Objects;
using Microsoft.AspNetCore.Identity;

namespace Domain.Contracts.Providers;

/// <summary>
/// Defines methods for providing FIDO2 passkey token operations for a specific user type.
/// </summary>
/// <typeparam name="TUser">The type representing a user, which must be a class with a parameterless constructor.</typeparam>
public interface IPasskeyTokenProvider<TUser> : IUserTwoFactorTokenProvider<TUser>
    where TUser : class
{
    /// <summary>
    /// Creates attestation options for registering credentials with FIDO2.
    /// </summary>
    /// <param name="user">The user for whom the attestation options are being created.</param>
    /// <param name="token">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the attestation options as a JSON string.</returns>
    Task<string> CreateAttestationOptionsAsync(TUser user, CancellationToken token = default);

    /// <summary>
    /// Creates assertion options for verifying credentials with FIDO2.
    /// </summary>
    /// <param name="user">The user for whom the assertion options are being created.</param>
    /// <param name="token">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the assertion options as a JSON string.</returns>
    Task<string> CreateAssertionOptionsAsync(TUser user, CancellationToken token = default);

    /// <summary>
    /// Verifies an assertion for a FIDO2 credential.
    /// </summary>
    /// <param name="user">The user for whom the assertion is being verified.</param>
    /// <param name="jsonOptions">The assertion options in JSON format used for verification.</param>
    /// <param name="responce">The raw response from the authenticator containing assertion data.</param>
    /// <param name="token">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the verification result as a JSON string.</returns>
    Task<AssertionVerificationResult> VerifyAssertionAsync(TUser user,
                                                           string jsonOptions,
                                                           AuthenticatorAssertionRawResponse responce,
                                                           CancellationToken token = default);

    /// <summary>
    /// Creates a new FIDO2 credential for the specified user based on the provided options and response.
    /// </summary>
    /// <param name="user">The user for whom the credential is being created.</param>
    /// <param name="jsonOptions">The attestation options in JSON format used for credential creation.</param>
    /// <param name="response">The raw response from the authenticator containing credential data.</param>
    /// <param name="token">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the asynchronous operation. The task result does not contain a value.</returns>
    Task CreateCredentialAsync(TUser user,
                               string jsonOptions,
                               AuthenticatorAttestationRawResponse response,
                               CancellationToken token = default);
}
