
using Application.Contracts;
using Application.Factories;
using Application.Results;
using Domain.Aggregates.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Stores;

/// <summary>
/// 
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
    public async Task<UserMultiFactorResult> SetEmailAsync(bool isEnabled, string userId)
    {
        try
        {
            var dbSet = DbContext.Set<UserMultiFactorSettings>();

            await dbSet.Where(x => x.UserId == userId)
                        .ExecuteUpdateAsync(x =>
                                            x.SetProperty(s => s.MultiFactorEmailEnabled, s => isEnabled));
            return UserMultiFactorResult.Success();
        }
        catch (Exception ex)
        {
            return UserMultiFactorResult.Failed(IdentityErrorFactory.ExceptionOccurred(ex));
        }

    }
    /// <inheritdoc/>
    public async Task<UserMultiFactorResult> SetAutheticatorAsync(bool isEnabled, string userId)
    {
        try
        {
            var dbSet = DbContext.Set<UserMultiFactorSettings>();

            await dbSet.Where(x => x.UserId == userId)
                        .ExecuteUpdateAsync(x =>
                                            x.SetProperty(s => s.MultiFactorAuthenticatorEnabled, s => isEnabled));
            return UserMultiFactorResult.Success();
        }
        catch (Exception ex)
        {
            return UserMultiFactorResult.Failed(IdentityErrorFactory.ExceptionOccurred(ex));
        }
    }
    /// <inheritdoc/>
    public async Task<UserMultiFactorResult> SetPasskeysAsync(bool isEnabled, string userId)
    {
        try
        {
            var dbSet = DbContext.Set<UserMultiFactorSettings>();

            await dbSet.Where(x => x.UserId == userId)
                        .ExecuteUpdateAsync(x =>
                                            x.SetProperty(s => s.MultiFactorPasskeysEnabled, s => isEnabled));
            return UserMultiFactorResult.Success();
        }
        catch (Exception ex)
        {
            return UserMultiFactorResult.Failed(IdentityErrorFactory.ExceptionOccurred(ex));
        }
    }
}