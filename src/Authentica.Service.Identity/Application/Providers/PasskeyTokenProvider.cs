using Application.DTOs;
using Domain.Contracts.Providers;
using Domain.Contracts.Stores;
using Fido2NetLib;
using Fido2NetLib.Objects;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace Application.Providers;

/// <summary>
/// Provides functionality for generating and verifying passkey tokens using the FIDO2 standard.
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
    /// <exception cref="ArgumentNullException">Thrown when any required dependency is null.</exception>
    public PasskeyTokenProvider(
        IFido2 fido2Lib,
        IPasskeyCredentialReadStore readStore,
        IPasskeyCredentialWriteStore writeStore)
    {
        Fido2Lib = fido2Lib ?? throw new ArgumentNullException(nameof(fido2Lib));
        ReadStore = readStore ?? throw new ArgumentNullException(nameof(readStore));
        WriteStore = writeStore ?? throw new ArgumentNullException(nameof(writeStore));
    }

    /// <inheritdoc/>
    public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
    {
        ArgumentNullException.ThrowIfNull(user);
        return Task.FromResult(true);
    }

    /// <inheritdoc/>
    public async Task<string> CreateAssertionOptionsAsync(TUser user, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(user);
        
        var credentials = await ReadStore.GetPasskeyCredentialsAsync(user.Id, token);
        var existingKeys = ConvertToPublicKeyDescriptors(credentials);
        
        var options = Fido2Lib.GetAssertionOptions(
            existingKeys, 
            UserVerificationRequirement.Discouraged);

        return options.ToJson();
    }

    /// <inheritdoc/>
    public async Task<string> CreateAttestationOptionsAsync(TUser user, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(user);
        
        var fidoUser = CreateFido2User(user);
        var credentials = await ReadStore.GetPasskeyCredentialsAsync(user.Id, token);
        var existingKeys = ConvertToPublicKeyDescriptors(credentials);

        var options = Fido2Lib.RequestNewCredential(
            fidoUser, 
            existingKeys, 
            AuthenticatorSelection.Default, 
            AttestationConveyancePreference.None);

        return options.ToJson();
    }

    /// <inheritdoc/>
    public async Task CreateCredentialAsync(
        TUser user,
        string jsonOptions,
        AuthenticatorAttestationRawResponse response,
        CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(jsonOptions);
        ArgumentNullException.ThrowIfNull(response);

        var options = CredentialCreateOptions.FromJson(jsonOptions);
        var uniquenessCallback = CreateUniqueCredentialCallback(user.Id);
        
        var result = await Fido2Lib.MakeNewCredentialAsync(
            response, 
            options, 
            uniquenessCallback, 
            cancellationToken: token);

        await WriteStore.CreateAsync(result);
    }

    /// <inheritdoc/>
    [Obsolete($"Please use {nameof(CreateCredentialAsync)}")]
    public Task<string> GenerateAsync(string purpose, UserManager<TUser> manager, TUser user)
    {
        throw new NotSupportedException($"This method is obsolete. Please use {nameof(CreateCredentialAsync)} instead.");
    }

    /// <inheritdoc/>
    [Obsolete($"Please use {nameof(VerifyAssertionAsync)}")]
    public Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser> manager, TUser user)
    {
        throw new NotSupportedException($"This method is obsolete. Please use {nameof(VerifyAssertionAsync)} instead.");
    }

    /// <inheritdoc/>
    public async Task<AssertionVerificationResult> VerifyAssertionAsync(
        TUser user,
        string jsonOptions,
        AuthenticatorAssertionRawResponse response,
        CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(jsonOptions);
        ArgumentNullException.ThrowIfNull(response);

        var options = AssertionOptions.FromJson(jsonOptions);
        var credentials = await ReadStore.GetPasskeyCredentialsAsync(user.Id, token);
        
        var credential = credentials.FirstOrDefault(c => c.PasskeyCredentialId.SequenceEqual(response.Id))
            ?? throw new InvalidOperationException("Credential not found for the provided response.");

        var ownershipCallback = CreateOwnershipCallback(credentials);
        
        var result = await Fido2Lib.MakeAssertionAsync(
            response, 
            options, 
            credential.PublicKey, 
            credential.SignatureCounter, 
            ownershipCallback, 
            cancellationToken: token);

        return result;
    }

    #region Private Helper Methods

    /// <summary>
    /// Creates a Fido2User from the provided user.
    /// </summary>
    private static Fido2User CreateFido2User(TUser user)
    {
        return new Fido2User
        {
            Id = Encoding.UTF8.GetBytes(user.Id),
            Name = user.Email ?? user.UserName ?? user.Id,
            DisplayName = user.Email ?? user.UserName ?? user.Id
        };
    }

    /// <summary>
    /// Converts a list of PasskeyCredentialReadDto to PublicKeyCredentialDescriptor list.
    /// </summary>
    private static List<PublicKeyCredentialDescriptor> ConvertToPublicKeyDescriptors(
        IEnumerable<PasskeyCredentialReadDto> credentials)
    {
        return credentials
            .Select(c => new PublicKeyCredentialDescriptor
            {
                Id = c.PasskeyCredentialId,
                Type = PublicKeyCredentialType.PublicKey
            })
            .ToList();
    }

    /// <summary>
    /// Creates a callback to check credential uniqueness for a user.
    /// </summary>
    private IsCredentialIdUniqueToUserAsyncDelegate CreateUniqueCredentialCallback(string userId)
    {
        return async (parameters, cancellationToken) =>
        {
            var existingCredentials = await ReadStore.GetPasskeyCredentialsAsync(userId, cancellationToken);
            return !existingCredentials.Any(c => c.PasskeyCredentialId.SequenceEqual(parameters.CredentialId));
        };
    }

    /// <summary>
    /// Creates a callback to verify ownership of credentials.
    /// </summary>
    private static IsUserHandleOwnerOfCredentialIdAsync CreateOwnershipCallback(
        List<PasskeyCredentialReadDto> credentials)
    {
        return (args, cancellationToken) =>
        {
            var isOwner = credentials.Any(c => c.PasskeyCredentialId.SequenceEqual(args.CredentialId));
            return Task.FromResult(isOwner);
        };
    }

    #endregion
}
