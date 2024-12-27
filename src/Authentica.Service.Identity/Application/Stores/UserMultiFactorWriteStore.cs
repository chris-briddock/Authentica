using Application.Factories;
using Application.Results;
using Domain.Aggregates.Identity;
using Domain.Contracts.Stores;
using Microsoft.EntityFrameworkCore;

namespace Application.Stores;

/// <summary>
/// Represents a store for user's multi factor preferences.
/// </summary>
public sealed class UserMultiFactorWriteStore : StoreBase, IUserMultiFactorWriteStore
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserMultiFactorWriteStore"/>
    /// </summary>
    /// <param name="services">The service provider used to resolve dependencies.</param>
    public UserMultiFactorWriteStore(IServiceProvider services) : base(services)
    {
    }
    /// <inheritdoc/>
    public async Task<UserMultiFactorStoreResult> SetEmailAsync(bool isEnabled, string userId)
    {
        try
        {
            var dbSet = DbContext.Set<UserMultiFactorSettings>();

            await dbSet.Where(x => x.UserId == userId)
                        .ExecuteUpdateAsync(x =>
                                            x.SetProperty(s => s.MultiFactorEmailEnabled, s => isEnabled));
            return UserMultiFactorStoreResult.Success();
        }
        catch (Exception ex)
        {
            return UserMultiFactorStoreResult.Failed(IdentityErrorFactory.ExceptionOccurred(ex));
        }

    }
    /// <inheritdoc/>
    public async Task<UserMultiFactorStoreResult> SetAutheticatorAsync(bool isEnabled, string userId)
    {
        try
        {
            var dbSet = DbContext.Set<UserMultiFactorSettings>();

            await dbSet.Where(x => x.UserId == userId)
                        .ExecuteUpdateAsync(x =>
                                            x.SetProperty(s => s.MultiFactorAuthenticatorEnabled, s => isEnabled));
            return UserMultiFactorStoreResult.Success();
        }
        catch (Exception ex)
        {
            return UserMultiFactorStoreResult.Failed(IdentityErrorFactory.ExceptionOccurred(ex));
        }
    }
    /// <inheritdoc/>
    public async Task<UserMultiFactorStoreResult> SetPasskeysAsync(bool isEnabled,
                                                              string userId)
    {
        try
        {
            var dbSet = DbContext.Set<UserMultiFactorSettings>();

            await dbSet.Where(x => x.UserId == userId)
                        .ExecuteUpdateAsync(x =>
                                            x.SetProperty(s => s.MultiFactorPasskeysEnabled, s => isEnabled));
            return UserMultiFactorStoreResult.Success();
        }
        catch (Exception ex)
        {
            return UserMultiFactorStoreResult.Failed(IdentityErrorFactory.ExceptionOccurred(ex));
        }
    }
}