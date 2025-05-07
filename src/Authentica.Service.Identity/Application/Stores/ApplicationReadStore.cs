using Application.Constants;
using Application.DTOs;
using Domain.Aggregates.Identity;
using Domain.Contracts.Stores;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using ZiggyCreatures.Caching.Fusion;

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

    /// <inheritdoc/>
    public async Task<bool> CheckApplicationExistsByNameAsync(string applicationName, CancellationToken cancellationToken = default)
    {
        // Define the compiled query
        var compiledQuery = EF.CompileAsyncQuery(
            (AppDbContext context, string name) =>
                context.Set<ClientApplication>().Any(a => a.Name == name)
        );

        return await compiledQuery(DbContext, applicationName);
    }

    /// <inheritdoc/>
    public async Task<ClientApplication?> GetClientApplicationByNameAndUserIdAsync(
    string name,
    string userId,
    CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);

        string cacheKey = $"clientApp_{name}_{userId}";

        // Define the compiled query
        var compiledQuery = EF.CompileAsyncQuery(
            (AppDbContext context, string appName, string uId) =>
                context.Set<ClientApplication>()
                    .Join(
                        context.Set<UserClientApplication>(),
                        app => app.Id,
                        userApp => userApp.ApplicationId,
                        (app, userApp) => new { app, userApp.UserId }
                    )
                    .Where(joined => joined.app.Name == appName && joined.UserId == uId)
                    .Select(joined => joined.app)
                    .FirstOrDefault()
        );

        // Use FusionCache for caching
        var clientApplication = await FusionCache.GetOrSetAsync<ClientApplication?>(
            cacheKey,
            async (ctx, ct) =>
            {
                ctx.Tags = [CacheTagConstants.Applications];
                // Query the database if the value is not found in the cache
                return await compiledQuery(DbContext, name, userId);
            },
            token: cancellationToken
        );

        return clientApplication;
    }


    /// <inheritdoc/>
    public async Task<ApplicationReadDto?> GetClientAppByClientIdAndCallbackUriAsync(string clientId,
                                                                                     string callbackUri,
                                                                                     CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(clientId);
        ArgumentException.ThrowIfNullOrWhiteSpace(callbackUri);

        var cacheKey = $"clientApp_{clientId}_{callbackUri}";

        // Define the compiled query
        var compiledQuery = EF.CompileAsyncQuery(
            (AppDbContext context, string cId, string cbUri) =>
                context.Set<ClientApplication>()
                    .Where(x => x.ClientId == cId && x.CallbackUri == cbUri)
                    .Select(x => new ApplicationReadDto
                    {
                        ClientId = x.ClientId,
                        EntityDeletionStatus = x.EntityDeletionStatus,
                        EntityCreationStatus = x.EntityCreationStatus,
                        EntityModificationStatus = x.EntityModificationStatus,
                        CallbackUri = x.CallbackUri,
                        Name = x.Name
                    })
                    .FirstOrDefault()
        );

        // Use FusionCache for caching
        var application = await FusionCache.GetOrSetAsync<ApplicationReadDto?>(
            cacheKey,
            async (ctx, ct) =>
            {
                ctx.Tags = [CacheTagConstants.Applications];
                // Query the database if the value is not found in the cache
                return await compiledQuery(DbContext, clientId, callbackUri);
            },
            options: new FusionCacheEntryOptions
            {
                Duration = TimeSpan.FromMinutes(10), // Cache duration
                IsFailSafeEnabled = true,            // Enable fail-safe mode
                FailSafeThrottleDuration = TimeSpan.FromSeconds(30), // Retry interval
            },
            token: cancellationToken
        );

        return application;
    }

    /// <inheritdoc/>
    public async Task<List<ApplicationReadDto>> GetAllClientApplicationsByUserIdAsync(
    string userId,
    CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);

        var cacheKey = $"clientApps_{userId}";

        // Define the compiled query
        var compiledQuery = EF.CompileAsyncQuery(
            (AppDbContext context, string uId) =>
                context.Set<ClientApplication>()
                    .Join(
                        context.Set<UserClientApplication>(),
                        app => app.Id,
                        userApp => userApp.ApplicationId,
                        (app, userApp) => new { app, userApp.UserId }
                    )
                    .Where(joined => joined.UserId == uId)
                    .Select(joined => new ApplicationReadDto
                    {
                        ClientId = joined.app.ClientId,
                        EntityDeletionStatus = joined.app.EntityDeletionStatus,
                        EntityCreationStatus = joined.app.EntityCreationStatus,
                        EntityModificationStatus = joined.app.EntityModificationStatus,
                        CallbackUri = joined.app.CallbackUri,
                        Name = joined.app.Name
                    })
                    .ToList()
        );

        // Use FusionCache for caching
        var clientApplications = await FusionCache.GetOrSetAsync<List<ApplicationReadDto>>(
            cacheKey,
            async (ctx, ct) =>
            {
                ctx.Tags = [CacheTagConstants.Applications];
                // Query the database if the value is not found in the cache
                return await compiledQuery(DbContext, userId);
            },
            token: cancellationToken
        );

        return clientApplications;
    }

    /// <inheritdoc/>
    public async Task<ApplicationReadDto> GetClientApplicationByClientIdAsync(string clientId,
                                                                              CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(clientId);

        var cacheKey = $"clientApp_{clientId}";

        // Define the compiled query
        var compiledQuery = EF.CompileAsyncQuery(
            (AppDbContext context, string cId) =>
                context.Set<ClientApplication>()
                    .Join(
                        context.Set<UserClientApplication>(),
                        client => client.Id,
                        userClient => userClient.ApplicationId,
                        (client, userClient) => new { client, userClient }
                    )
                    .Where(x => x.client.ClientId == cId)
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
                    .First()
        );

        // Use FusionCache to manage caching
        var result = await FusionCache.GetOrSetAsync<ApplicationReadDto>(
            cacheKey,
            async (ctx, ct) =>
            {
                ctx.Tags = [CacheTagConstants.Applications];
                // Query the database if the value is not found in the cache
                return await compiledQuery(DbContext, clientId);
            },
            options: new FusionCacheEntryOptions
            {
                Duration = TimeSpan.FromMinutes(10), // Cache duration
                IsFailSafeEnabled = true,            // Enable fail-safe mode
                FailSafeThrottleDuration = TimeSpan.FromSeconds(30), // Retry interval
            },
            token: cancellationToken
        );

        return result;
    }

    /// <inheritdoc/>
    public async Task<bool> CheckApplicationExistsByClientIdAsync(string clientId, CancellationToken cancellationToken = default)
    {
        // Define the compiled query
        var compiledQuery = EF.CompileAsyncQuery(
            (AppDbContext context, string cId) =>
                context.Set<ClientApplication>().Any(x => x.ClientId == cId)
        );

        return await compiledQuery(DbContext, clientId);
    }

    /// <inheritdoc/>
    public async Task<List<ApplicationReadDto>> GetAllApplicationsAsync(CancellationToken cancellationToken = default)
    {
        var cacheKey = "allApplications";

        // Define the compiled query
        var compiledQuery = EF.CompileAsyncQuery(
            (AppDbContext context) =>
                context.ClientApplications
                    .Select(x => new ApplicationReadDto
                    {
                        ClientId = x.ClientId,
                        EntityCreationStatus = x.EntityCreationStatus,
                        EntityDeletionStatus = x.EntityDeletionStatus,
                        EntityModificationStatus = x.EntityModificationStatus,
                        CallbackUri = x.CallbackUri,
                        Name = x.Name
                    })
                    .ToList()
        );

        var result = await FusionCache.GetOrSetAsync<List<ApplicationReadDto>>(
            cacheKey,
            async (ctx, ct) =>
            {
                ctx.Tags = [CacheTagConstants.Applications];
                // Query the database if the value is not found in the cache
                return await compiledQuery(DbContext);
            },
            token: cancellationToken
        );

        return result;
    }
}