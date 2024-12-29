using Application.DTOs;
using Domain.Contracts.Providers;
using Domain.Contracts.Stores;
using Fido2NetLib;
using Fido2NetLib.Objects;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace Application.Providers;

/// <summary>
/// Provides functionality for generating and verifying passkey tokens.
/// </summary>
/// <typeparam name="TUser">The type of user.</typeparam>
public sealed class PasskeyTokenProvider<TUser> : IPasskeyTokenProvider<TUser> 
    where TUser : IdentityUser<string>
{
    /// <summary>
    /// Gets the Fido2 library instance.
    /// </summary>
    private IFido2 Fido2Lib { get; }

    /// <summary>
    /// Gets the passkey credential read store instance.
    /// </summary>
    private IPasskeyCredentialReadStore ReadStore { get; }

    /// <summary>
    /// Gets the passkey credential write store instance.
    /// </summary>
    private IPasskeyCredentialWriteStore WriteStore { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PasskeyTokenProvider{TUser}"/> class.
    /// </summary>
    /// <param name="fido2Lib">The Fido2 library instance.</param>
    /// <param name="readStore">The passkey credential read store instance.</param>
    /// <param name="writeStore">The passkey credential write store instance.</param>
    public PasskeyTokenProvider(IFido2 fido2Lib,
                                IPasskeyCredentialReadStore readStore,
                                IPasskeyCredentialWriteStore writeStore)
    {
        Fido2Lib = fido2Lib ?? throw new ArgumentNullException(nameof(fido2Lib));
        ReadStore = readStore ?? throw new ArgumentNullException(nameof(readStore));
        WriteStore = writeStore ?? throw new ArgumentNullException(nameof(writeStore));

    }

    /// <summary>
    /// Determines whether a two-factor token can be generated for the specified user.
    /// </summary>
    /// <param name="manager">The user manager instance.</param>
    /// <param name="user">The user instance.</param>
    /// <returns>A task that represents the asynchronous operation. The task result indicates whether a two-factor token can be generated.</returns>
    public async Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
    {
        return await Task.FromResult(true);
    }

    /// <summary>
    /// Creates assertion options for the specified user.
    /// </summary>
    /// <param name="user">The user instance.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is the assertion options as a JSON string.</returns>
    public async Task<string> CreateAssertionOptionsAsync(TUser user, CancellationToken token = default)
    {
        List<PublicKeyCredentialDescriptor> existingKeys = [];

        var credentials = await ReadStore.GetPasskeyCredentialsAsync(user.Id, token);

        foreach (var credential in credentials) 
        {
            existingKeys.Add(new PublicKeyCredentialDescriptor()
            {
                Id = credential.PasskeyCredentialId,
                Type = PublicKeyCredentialType.PublicKey
            });
        }  

        AssertionOptions options = Fido2Lib.GetAssertionOptions(existingKeys, UserVerificationRequirement.Discouraged);

        return options.ToJson();
    }

    /// <summary>
    /// Creates attestation options for the specified user.
    /// </summary>
    /// <param name="user">The user instance.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is the attestation options as a JSON string.</returns>
    public async Task<string> CreateAttestationOptionsAsync(TUser user, CancellationToken token = default)
    {
        Fido2User fidoUser = new()
        {
            Id = Encoding.UTF8.GetBytes(user.Id),
            Name = user.Email,
            DisplayName = user.Email
        };

        List<PublicKeyCredentialDescriptor> existingKeys = [];

        List<PasskeyCredentialReadDto> query = await ReadStore.GetPasskeyCredentialsAsync(user.Id, token);

        foreach (var credential in query)
        {
            existingKeys.Add(new PublicKeyCredentialDescriptor()
            {
                Id = credential.PasskeyCredentialId,
                Type = PublicKeyCredentialType.PublicKey
            });
        }

        var options = Fido2Lib.RequestNewCredential(fidoUser, existingKeys, AuthenticatorSelection.Default, AttestationConveyancePreference.None);

        return options.ToJson();

        // at this point in the controller, add these options to the httpcontext session.

    }

    /// <summary>
    /// Creates a credential for the specified user.
    /// </summary>
    /// <param name="user">The user instance.</param>
    /// <param name="jsonOptions">The JSON options.</param>
    /// <param name="response">The authenticator attestation raw response.</param>
    /// <param name="token">The cancellation token.</param>
    public async Task CreateCredentialAsync(TUser user,
                                            string jsonOptions,
                                            AuthenticatorAttestationRawResponse response,
                                            CancellationToken token = default)
    {
        var options = CredentialCreateOptions.FromJson(jsonOptions);

        IsCredentialIdUniqueToUserAsyncDelegate callback = async (parameters, ctx) =>
        {
            List<PasskeyCredentialReadDto> credentials = await ReadStore.GetPasskeyCredentialsAsync(user.Id, ctx);
            if (credentials.Count > 0) return false;

            return true;
        };
        // 2. Verify and make the credentials
        Fido2.CredentialMakeResult result = await Fido2Lib.MakeNewCredentialAsync(response, options, callback);

        // persist the credential.
        await WriteStore.CreateAsync(result);
    }

    /// <summary>
    /// Generates a passkey token for the specified user. This method is obsolete and should not be used.
    /// </summary>
    /// <param name="purpose">The purpose of the token.</param>
    /// <param name="manager">The user manager instance.</param>
    /// <param name="user">The user instance.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is the generated token.</returns>
    [Obsolete($"Please use {nameof(CreateCredentialAsync)}")]
    public Task<string> GenerateAsync(string purpose, UserManager<TUser> manager, TUser user)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Validates a passkey token for the specified user. This method is obsolete and should not be used.
    /// </summary>
    /// <param name="purpose">The purpose of the token.</param>
    /// <param name="token">The token to validate.</param>
    /// <param name="manager">The user manager instance.</param>
    /// <param name="user">The user instance.</param>
    /// <returns>A task that represents the asynchronous operation. The task result indicates whether the token is valid.</returns>
    [Obsolete($"Please use {nameof(VerifyAssertionAsync)}")]
    public Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser> manager, TUser user)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Verifies an assertion for the specified user.
    /// </summary>
    /// <param name="user">The user instance.</param>
    /// <param name="jsonOptions">The JSON options.</param>
    /// <param name="response">The authenticator assertion raw response.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is the assertion verification result.</returns>
    public async Task<AssertionVerificationResult> VerifyAssertionAsync(
        TUser user,
        string jsonOptions,
        AuthenticatorAssertionRawResponse response,
        CancellationToken token = default)
    {
        var options = AssertionOptions.FromJson(jsonOptions);

        uint signatureCount = 0;

        PasskeyCredentialReadDto credential = new();

        List<PasskeyCredentialReadDto> result = await ReadStore.GetPasskeyCredentialsAsync(user.Id, token);

        for (int i = 0; i < result.Count; i++)
        {  
            credential = result[i];
            if (credential.PasskeyCredentialId.SequenceEqual(response.Id))
            {
                signatureCount = credential.SignatureCounter;
                break;
            }
        }

        IsUserHandleOwnerOfCredentialIdAsync callback = async (args, cancellationToken) =>
        {
            var storedCreds = result;
            return await Task.FromResult(storedCreds.Exists(c => c.PasskeyCredentialId.SequenceEqual(args.CredentialId)));
        };
        var res = await Fido2Lib.MakeAssertionAsync(response, options, credential.PublicKey, signatureCount, callback);

        // update the counter

        // return the result

        return res;
    }
}
