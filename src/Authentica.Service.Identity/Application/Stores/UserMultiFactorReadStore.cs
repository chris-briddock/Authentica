using Application.DTOs;
using Domain.Aggregates.Identity;
using Domain.Contracts.Stores;
using Microsoft.EntityFrameworkCore;

namespace Application.Stores;

/// <summary>
/// Represents a store for user's multi factor preferences.
/// </summary>
public sealed class UserMultiFactorReadStore : StoreBase, IUserMultiFactorReadStore
{
    private DbSet<UserMultiFactorSettings> DbSet => DbContext.Set<UserMultiFactorSettings>();
    /// <summary>
    /// Initializes a new instance of the <see cref="UserMultiFactorReadStore"/>
    /// </summary>
    /// <param name="services">The service provider used to resolve dependencies.</param>
    public UserMultiFactorReadStore(IServiceProvider services) : base(services)
    {
    }

    /// <inheritdoc/>
    public async Task<UserMultiFactorReadDto> GetAsync(string userId)
    {
        try
        {
            var settings = await DbSet
                .Where(x => x.UserId == userId)
                .Select(x => new UserMultiFactorReadDto
                {
                    MultiFactorEmailEnabled = x.MultiFactorEmailEnabled,
                    MultiFactorAuthenticatorEnabled = x.MultiFactorAuthenticatorEnabled,
                    MultiFactorPasskeysEnabled = x.MultiFactorPasskeysEnabled
                })
                .FirstOrDefaultAsync();

            return settings ?? null! ;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to get user multi factor settings", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<bool> IsEmailEnabledAsync(string userId)
    {
        try
        {
            var settings = await DbSet
                .Where(x => x.UserId == userId)
                .Select(x => x.MultiFactorEmailEnabled)
                .FirstOrDefaultAsync();

            return settings;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to check if email is enabled", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<bool> IsAuthenticatorEnabledAsync(string userId)
    {
        try
        {
            bool settings = await DbSet
                .Where(x => x.UserId == userId)
                .Select(x => x.MultiFactorAuthenticatorEnabled)
                .FirstOrDefaultAsync();

            return settings;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to check if authenticator is enabled", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<bool> IsPasskeysEnabledAsync(string userId)
    {
        try
        {
            bool settings = await DbSet
                .Where(x => x.UserId == userId)
                .Select(x => x.MultiFactorPasskeysEnabled)
                .FirstOrDefaultAsync();

            return settings;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to check if passkeys is enabled", ex);
        }
    }
}
