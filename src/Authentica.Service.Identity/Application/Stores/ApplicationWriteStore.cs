using Application.Constants;
using Application.Factories;
using Application.Results;
using Domain.Aggregates.Identity;
using Domain.Contracts.Cryptography;
using Domain.Contracts.Providers;
using Domain.Contracts.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Application.Stores;

/// <summary>
/// Handles write operations related to creating new client applications
/// and updating existing client applications.
/// </summary>
public class ApplicationWriteStore : StoreBase, IApplicationWriteStore
{

    private readonly ILogger<ApplicationWriteStore> _logger;
    /// <summary>
    /// Gets the App DbSet.
    /// </summary>
    private DbSet<ClientApplication> MainDbSet => DbContext.Set<ClientApplication>();
    private DbSet<UserClientApplication> LinkDbSet => DbContext.Set<UserClientApplication>();

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationWriteStore"/> class.
    /// </summary>
    /// <param name="services">The service provider to retrieve required services for the write store operations.</param>
    public ApplicationWriteStore(IServiceProvider services) : base(services)
    {
        _logger = services.GetRequiredService<ILogger<ApplicationWriteStore>>();
        FusionCache.RemoveByTagAsync(CacheTagConstants.Applications);
    }
    /// <inheritdoc/>
    public async Task<ApplicationStoreResult> CreateClientApplicationAsync(ClaimsPrincipal claimsPrincipal,
                                                                          string name,
                                                                          string callbackUri,
                                                                          CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(callbackUri);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        try
        {
            var user = (await UserReadStore.GetUserByEmailAsync(claimsPrincipal, cancellationToken)).User;

            var application = new ClientApplication
            {
                Id = Ulid.NewUlid().ToString(),
                Name = name,
                CallbackUri = callbackUri,
                EntityDeletionStatus = new(false, null, null),
                EntityModificationStatus = new(DateTime.UtcNow, user.Id),
                EntityCreationStatus = new(DateTime.UtcNow, user.Id)
            };

            var userClientApplication = new UserClientApplication
            {
                UserId = user.Id,
                ApplicationId = application.Id
            };

            MainDbSet.Add(application);
            LinkDbSet.Add(userClientApplication);
            await DbContext.SaveChangesAsync(cancellationToken);

            return ApplicationStoreResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating client application {Name} for user {Email}", name, claimsPrincipal.Identity?.Name ?? "unknown");
            return ApplicationStoreResult.Failed(IdentityErrorFactory.ExceptionOccurred(ex));
        }
    }


    /// <inheritdoc/>
    public async Task<ApplicationStoreResult> UpdateApplicationAsync(ClaimsPrincipal claimsPrincipal,
                                                                     string? oldName,
                                                                     string? newName,
                                                                     string? callbackUri,
                                                                     CancellationToken cancellationToken = default)
    {
        try
        {
            var userReadResult = await UserReadStore.GetUserByEmailAsync(claimsPrincipal, cancellationToken);

            if (userReadResult.User is null)
            {
                return ApplicationStoreResult.Failed(IdentityErrorFactory.UserNotFound());
            }

            var application = await ApplicationReadStore.GetClientApplicationByNameAndUserIdAsync(oldName!,
                                                                                                  userReadResult.User.Id,
                                                                                                  cancellationToken);

            if (application is null)
            {
                return ApplicationStoreResult.Failed(IdentityErrorFactory.ApplicationNotFound());
            }

            application.Name = newName ?? application.Name;
            application.CallbackUri = callbackUri ?? application.CallbackUri;
            application.EntityModificationStatus.ModifiedBy = userReadResult.User.Email ?? application.EntityModificationStatus.ModifiedBy;
            application.EntityModificationStatus.ModifiedOnUtc = DateTime.UtcNow;

            MainDbSet.Update(application);
            await DbContext.SaveChangesAsync(cancellationToken);

            return ApplicationStoreResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating application from {OldName} to {NewName} for user {Email}", oldName ?? "unknown", newName ?? "unchanged", claimsPrincipal.Identity?.Name ?? "unknown");
            return ApplicationStoreResult.Failed(IdentityErrorFactory.ExceptionOccurred(ex));
        }
    }


    /// <inheritdoc/>
    public async Task<ApplicationStoreResult> SoftDeleteApplicationAsync(ClaimsPrincipal claimsPrincipal,
                                                                         string name,
                                                                         CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(claimsPrincipal);

        try
        {
            var userReadResult = await UserReadStore.GetUserByEmailAsync(claimsPrincipal, cancellationToken);

            // Check if the user exists
            if (userReadResult.User is null)
            {
                return ApplicationStoreResult.Failed(IdentityErrorFactory.UserNotFound());
            }

            // Retrieve the application to be deleted based on the provided name and user ID
            var application = await ApplicationReadStore.GetClientApplicationByNameAndUserIdAsync(name,
                                                                                                  userReadResult.User.Id,
                                                                                                  cancellationToken);

            // Check if the application exists
            if (application is null)
            {
                return ApplicationStoreResult.Failed(IdentityErrorFactory.ApplicationNotFound());
            }

            // Mark the application as deleted and set the deletion details
            application.EntityDeletionStatus.IsDeleted = true;
            application.EntityDeletionStatus.DeletedOnUtc = DateTime.UtcNow;
            application.EntityDeletionStatus.DeletedBy = userReadResult.User.Id;

            // Execute the SQL command to update the application
            MainDbSet.Update(application);
            await DbContext.SaveChangesAsync(cancellationToken);

            // Return success result
            return ApplicationStoreResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error soft deleting application {Name} for user {Email}", name, claimsPrincipal.Identity?.Name ?? "unknown");
            // Return failure result in case of an exception
            return ApplicationStoreResult.Failed(IdentityErrorFactory.ExceptionOccurred(ex));
        }
    }


    /// <inheritdoc/>
    public async Task<ApplicationStoreResult> UpdateClientSecretAsync(ClaimsPrincipal claimsPrincipal,
                                                                      string applicationName,
                                                                      CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(claimsPrincipal);

        try
        {
            var randomStringProvider = Services.GetRequiredService<IRandomStringProvider>();
            var hasher = Services.GetRequiredService<ISecretHasher>();

            var userReadResult = await UserReadStore.GetUserByEmailAsync(claimsPrincipal, cancellationToken);

            // Check if the user exists
            if (userReadResult.User is null)
            {
                return ApplicationStoreResult.Failed(IdentityErrorFactory.UserNotFound());
            }

            // Retrieve the application to be updated based on the provided name and user ID.
            var application = await ApplicationReadStore.GetClientApplicationByNameAndUserIdAsync(applicationName,
                                                                                                  userReadResult.User.Id,
                                                                                                  cancellationToken);

            // Check if the application exists
            if (application is null)
            {
                return ApplicationStoreResult.Failed(IdentityErrorFactory.ApplicationNotFound());
            }

            var secret = randomStringProvider.GenerateAlphanumeric();

            var hashedSecret = hasher.Hash(secret);

            // Update the client secret
            application.ClientSecret = hashedSecret;
            MainDbSet.Update(application);
            await DbContext.SaveChangesAsync(cancellationToken);

            // Return success result
            return ApplicationStoreResult.Success(secret);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating client secret for application {ApplicationName} for user {Email}", applicationName, claimsPrincipal.Identity?.Name ?? "unknown");
            // Return failure result in case of an exception
            return ApplicationStoreResult.Failed(IdentityErrorFactory.ExceptionOccurred(ex));
        }
    }
}