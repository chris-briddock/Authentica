using Application.Constants;
using Application.Factories;
using Application.Results;
using Domain.Aggregates.Identity;
using Domain.Contracts.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Stores;

/// <summary>
/// Represents a store for user's multi factor preferences.
/// </summary>
public sealed class UserMultiFactorWriteStore : StoreBase, IUserMultiFactorWriteStore
{
    private readonly ILogger<UserMultiFactorWriteStore> _logger;
    private DbSet<UserMultiFactorSettings> DbSet => DbContext.Set<UserMultiFactorSettings>();
    /// <summary>
    /// Initializes a new instance of the <see cref="UserMultiFactorWriteStore"/>
    /// </summary>
    /// <param name="services">The service provider used to resolve dependencies.</param>
    public UserMultiFactorWriteStore(IServiceProvider services) : base(services)
    {
        _logger = services.GetRequiredService<ILogger<UserMultiFactorWriteStore>>();
        FusionCache.RemoveByTag(CacheTagConstants.MultiFactorSettings);
    }
    /// <inheritdoc/>
    public async Task<UserMultiFactorStoreResult> CreateAsync(string userId)
    {
        try
        {

            UserMultiFactorSettings settings = new()
            {
                UserId = userId,
                MultiFactorEmailEnabled = false,
                MultiFactorAuthenticatorEnabled = false,
                MultiFactorPasskeysEnabled = false,
                EntityCreationStatus = new(DateTime.UtcNow, userId),
                EntityModificationStatus = new(DateTime.UtcNow, userId)
            };
            
            await DbSet.AddAsync(settings);

            await DbContext.SaveChangesAsync();

            return UserMultiFactorStoreResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating multi-factor settings for user {UserId}", userId);
            return UserMultiFactorStoreResult.Failed(IdentityErrorFactory.ExceptionOccurred(ex));
        }
    }
    /// <inheritdoc/>
    public async Task<UserMultiFactorStoreResult> SetEmailAsync(bool isEnabled, string userId)
    {
        try
        {
            await DbSet.Where(x => x.UserId == userId)
                        .ExecuteUpdateAsync(x =>
                                            x.SetProperty(s => s.MultiFactorEmailEnabled, s => isEnabled));
            return UserMultiFactorStoreResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting email multi-factor to {IsEnabled} for user {UserId}", isEnabled, userId);
            return UserMultiFactorStoreResult.Failed(IdentityErrorFactory.ExceptionOccurred(ex));
        }

    }
    /// <inheritdoc/>
    public async Task<UserMultiFactorStoreResult> SetAutheticatorAsync(bool isEnabled, string userId)
    {
        try
        {
            await DbSet.Where(x => x.UserId == userId)
                        .ExecuteUpdateAsync(x =>
                                            x.SetProperty(s => s.MultiFactorAuthenticatorEnabled, s => isEnabled));
            return UserMultiFactorStoreResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting authenticator multi-factor to {IsEnabled} for user {UserId}", isEnabled, userId);
            return UserMultiFactorStoreResult.Failed(IdentityErrorFactory.ExceptionOccurred(ex));
        }
    }
    /// <inheritdoc/>
    public async Task<UserMultiFactorStoreResult> SetPasskeysAsync(bool isEnabled,
                                                                   string userId)
    {
        try
        {
            await DbSet.Where(x => x.UserId == userId)
                        .ExecuteUpdateAsync(x =>
                                            x.SetProperty(s => s.MultiFactorPasskeysEnabled, s => isEnabled));
            return UserMultiFactorStoreResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting passkeys multi-factor to {IsEnabled} for user {UserId}", isEnabled, userId);
            return UserMultiFactorStoreResult.Failed(IdentityErrorFactory.ExceptionOccurred(ex));
        }
    }
}