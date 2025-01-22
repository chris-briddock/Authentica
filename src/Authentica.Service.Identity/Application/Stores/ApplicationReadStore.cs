using System.Text.Json;
using Application.DTOs;
using Domain.Aggregates.Identity;
using Domain.Contracts.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

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

        string cacheKey = $"clientApp_{name}_{userId}";

        // Check in-memory cache
        if (MemoryCache.TryGetValue(cacheKey, out ClientApplication? cachedApplication))
            return cachedApplication;

        // Check redis cache
        if (IsRedisEnabled)
        {
            var redisData = await DistributedCache.GetStringAsync(cacheKey, cancellationToken);

            if (redisData is not null)
            {
                cachedApplication = JsonSerializer.Deserialize<ClientApplication>(redisData);
                MemoryCache.Set(cacheKey, cachedApplication);
                return cachedApplication;
            }
        }

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

        // Cache the result
        if (clientApplication != null)
        {
            MemoryCache.Set(cacheKey, clientApplication);
            if (IsRedisEnabled)
                await DistributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(clientApplication), cancellationToken);
        }

        return clientApplication;
    }

    /// <inheritdoc/>
    public async Task<ApplicationReadDto?> GetClientAppByClientIdAndCallbackUriAsync(string clientId,
                                                                                        string callbackUri,
                                                                                        CancellationToken cancellationToken = default!)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(clientId);
        ArgumentException.ThrowIfNullOrWhiteSpace(callbackUri);

        var cacheKey = $"clientApp_{clientId}_{callbackUri}";

        // Check in-memory cache
        if (MemoryCache.TryGetValue(cacheKey, out ApplicationReadDto? cachedApplication))
            return cachedApplication;
        
        // Check redis cache
        if (IsRedisEnabled)
        {
            var redisData = await DistributedCache.GetStringAsync(cacheKey, cancellationToken);

            if (redisData is not null)
            {
                cachedApplication = JsonSerializer.Deserialize<ApplicationReadDto>(redisData);
                MemoryCache.Set(cacheKey, cachedApplication);
                return cachedApplication;
            }
        }

        var application = await MainDbSet
            .Where(x => x.ClientId == clientId && x.CallbackUri == callbackUri)
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

        // Cache the result
        if (application is not null)
        {
            MemoryCache.Set(cacheKey, application);
            if (IsRedisEnabled)
                await DistributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(application), cancellationToken);
        }

        return application;
    }

    /// <inheritdoc/>
    public async Task<List<ApplicationReadDto>> GetAllClientApplicationsByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);

        var cacheKey = $"clientApps_{userId}";

        // Check in-memory cache.
        if (MemoryCache.TryGetValue(cacheKey, out List<ApplicationReadDto>? applications))
            return applications!;
        
        // Check redis cache.
        if (IsRedisEnabled)
        {
            var redisData = await DistributedCache.GetStringAsync(cacheKey);

            if (redisData is not null)
            {
                applications = JsonSerializer.Deserialize<List<ApplicationReadDto>>(redisData);
                MemoryCache.Set(cacheKey, applications);
                return applications!;
            }
        }
        // Query the database if both caches are empty.
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
            }).ToListAsync(cancellationToken);

        // Cache the result in memory.
        MemoryCache.Set(cacheKey, clientApplications);
        // Cache the result in redis.
        if (IsRedisEnabled)
            await DistributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(clientApplications), cancellationToken);

        return clientApplications;
    }

    /// <inheritdoc/>
    public async Task<ApplicationReadDto> GetClientApplicationByClientIdAsync(string clientId, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"clientApp_{clientId}";

        // Check in-memory cache
        if (MemoryCache.TryGetValue(cacheKey, out ApplicationReadDto? applications))
            return applications!;

        // Check redis cache.
        if (IsRedisEnabled)
        {
            var redisData = DistributedCache.GetString(cacheKey);

            if (redisData is not null)
            {
                applications = JsonSerializer.Deserialize<ApplicationReadDto>(redisData);
                MemoryCache.Set(cacheKey, applications);
                return applications!;
            }
        }

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

        // Cache the result
        MemoryCache.Set(cacheKey, result);
        if (IsRedisEnabled)
            await DistributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result), cancellationToken);

        return result;
    }

    /// <inheritdoc/>
    public async Task<bool> CheckApplicationExistsByClientIdAsync(string clientId, CancellationToken cancellationToken = default)
    {
        return await MainDbSet.AnyAsync(x => x.ClientId == clientId, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<List<ApplicationReadDto>> GetAllApplicationsAsync(CancellationToken cancellationToken = default)
    {
        var cacheKey = "allApplications";

        // Check in-memory cache
        if (MemoryCache.TryGetValue(cacheKey, out List<ApplicationReadDto>? applications))
            return applications!;

        // Check redis cache.
        if (IsRedisEnabled)
        {
            var redisData = DistributedCache.GetString(cacheKey);

            if (redisData is not null)
            {
                applications = JsonSerializer.Deserialize<List<ApplicationReadDto>>(redisData);
                MemoryCache.Set(cacheKey, applications);
                return applications!;
            }
        }
        var result = await DbContext.ClientApplications
                                  .Select(x => new ApplicationReadDto
                                  {
                                      ClientId = x.ClientId,
                                      EntityCreationStatus = x.EntityCreationStatus,
                                      EntityDeletionStatus = x.EntityDeletionStatus,
                                      EntityModificationStatus = x.EntityModificationStatus,
                                      CallbackUri = x.CallbackUri,
                                      Name = x.Name
                                  }).ToListAsync(cancellationToken);

        // Cache the result
        MemoryCache.Set(cacheKey, applications);
        if (IsRedisEnabled)
            await DistributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(applications), cancellationToken);

        return result;
    }
}