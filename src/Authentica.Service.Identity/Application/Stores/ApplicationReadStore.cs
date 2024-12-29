using Application.DTOs;
using Domain.Aggregates.Identity;
using Domain.Contracts.Stores;
using Microsoft.EntityFrameworkCore;

namespace Application.Stores;

/// <summary>
/// Provides read operations for application-related data.
/// </summary>
public sealed class ApplicationReadStore : StoreBase, IApplicationReadStore
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationReadStore"/> class.
    /// </summary>
    /// <param name="services">The service provider used to resolve dependencies.</param>
    public ApplicationReadStore(IServiceProvider services) : base(services)
    {
    }

    /// <summary>
    /// Gets the ClientApplication DbSet.
    /// </summary>
    public DbSet<ClientApplication> MainDbSet => DbContext.Set<ClientApplication>();

    /// <summary>
    /// Gets the UserClientApplication DbSet.
    /// </summary>
    public DbSet<UserClientApplication> LinkDbSet => DbContext.Set<UserClientApplication>();

    /// <inheritdoc/>
    public async Task<bool> CheckApplicationExistsByNameAsync(string applicationName, CancellationToken cancellationToken = default)
    {
        return await MainDbSet.AnyAsync(a => a.Name == applicationName, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<ClientApplication?> GetClientApplicationByNameAndUserIdAsync(string name,
                                                                                    string userId,
                                                                                    CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);

        var clientApplication = await MainDbSet
            .Join(
                LinkDbSet,
                app => app.Id,
                userApp => userApp.ApplicationId,
                (app, userApp) => new { app, userApp.UserId }
            )
            .Where(joined => joined.app.Name == name && joined.UserId == userId)
            .Select(joined => joined.app)
            .FirstOrDefaultAsync(cancellationToken);

        return clientApplication;
    }
    /// <inheritdoc/>
    public async Task<ApplicationReadDto?> GetClientApplicationByClientIdAndCallbackUri(string clientId,
                                                                                        string callbackUri,
                                                                                        CancellationToken cancellationToken = default!)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(clientId);
        ArgumentException.ThrowIfNullOrWhiteSpace(callbackUri);

        return await MainDbSet
            .Where(x => x.ClientId == clientId)
            .Where(x => x.CallbackUri == callbackUri)
            .Select(x => new ApplicationReadDto
            {
                ClientId = x.ClientId,
                EntityDeletionStatus = x.EntityDeletionStatus,
                EntityCreationStatus = x.EntityCreationStatus,
                EntityModificationStatus = x.EntityModificationStatus,
                CallbackUri = x.CallbackUri,
                Name = x.Name
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IList<ApplicationReadDto>> GetAllClientApplicationsByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);

        var clientApplications = await MainDbSet
            .Join(
                LinkDbSet,
                app => app.Id,
                userApp => userApp.ApplicationId,
                (app, userApp) => new { app, userApp.UserId }
            )
            .Where(joined => joined.UserId == userId)
            .Select(joined => new ApplicationReadDto
            {
                ClientId = joined.app.ClientId,
                EntityDeletionStatus = joined.app.EntityDeletionStatus,
                EntityCreationStatus = joined.app.EntityCreationStatus,
                EntityModificationStatus = joined.app.EntityModificationStatus,
                CallbackUri = joined.app.CallbackUri,
                Name = joined.app.Name
            })
            .ToListAsync(cancellationToken);

        return clientApplications;
    }
    /// <inheritdoc/>
    public async Task<ApplicationReadDto> GetClientApplicationByClientId(string clientId, CancellationToken cancellationToken = default)
    {
        var result = await MainDbSet.Join(LinkDbSet,
                                        client => client.Id,
                                        userClient => userClient.ApplicationId,
                                        (client, userClient) => new { client, userClient })
                                    .Where(x => x.client.ClientId == clientId)
                                    .Select(x => new ApplicationReadDto
                                    {
                                        CallbackUri = x.client.CallbackUri,
                                        EntityDeletionStatus = x.client.EntityDeletionStatus,
                                        EntityModificationStatus = x.client.EntityModificationStatus,
                                        EntityCreationStatus = x.client.EntityCreationStatus,
                                        Name = x.client.Name,
                                        ClientId = x.client.ClientId,
                                        ClientSecret = x.client.ClientSecret,
                                        UserId = x.userClient.UserId
                                    })
                                   .FirstAsync(cancellationToken);

        return result;
    }
    /// <inheritdoc/>
    public async Task<bool> CheckApplicationExistsByClientIdAsync(string clientId, CancellationToken cancellationToken = default)
    {
        return await MainDbSet.AnyAsync(x => x.ClientId == clientId, cancellationToken);
    }
}
