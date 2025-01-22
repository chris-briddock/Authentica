using Authentica.Common;
using Domain.Aggregates.Identity;
using Domain.Contracts.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using Persistence.Contexts;

namespace Application.Stores;

/// <summary>
/// Provides a base class for data stores with common service dependencies.
/// </summary>
public abstract class StoreBase
{
    /// <summary>
    /// Gets the service provider used to resolve dependencies.
    /// </summary>
    public IServiceProvider Services { get; }

    /// <summary>
    /// Gets the <see cref="AppDbContext"/> instance from the service provider.
    /// This context is used to interact with the application's database.
    /// </summary>
    public AppDbContext DbContext => Services.GetRequiredService<AppDbContext>();

    /// <summary>
    /// Gets the <see cref="HttpContext"/> instance from the service provider.
    /// </summary>
    public HttpContext HttpContext => Services.GetRequiredService<IHttpContextAccessor>().HttpContext!;

    /// <summary>
    /// Gets the <see cref="UserManager{User}"/> instance from the service provider.
    /// This manager is used to manage user-related operations.
    /// </summary>
    public UserManager<User> UserManager => Services.GetRequiredService<UserManager<User>>();

    /// <summary>
    /// Gets the <see cref="IApplicationReadStore"/> instance from the service provider.
    /// This store provides read operations for application-related data.
    /// </summary>
    public IApplicationReadStore ApplicationReadStore => Services.GetRequiredService<IApplicationReadStore>();

    /// <summary>
    /// Gets the <see cref="IUserReadStore"/> instance from the service provider.
    /// This store provides read operations for user-related data.
    /// </summary>
    public IUserReadStore UserReadStore => Services.GetRequiredService<IUserReadStore>();

    /// <summary>
    /// Gets the emory cache.
    /// </summary>
    public IMemoryCache MemoryCache => Services.GetRequiredService<IMemoryCache>();
    /// <summary>
    /// Gets the distributed cache.
    /// </summary>
    public IDistributedCache DistributedCache => Services.GetRequiredService<IDistributedCache>();
    /// <summary>
    /// Gets the feature manager.
    /// </summary>
    public IFeatureManager FeatureManager => Services.GetRequiredService<IFeatureManager>();
    /// <summary>
    /// Gets a value indicating whether Redis is enabled in the feature flags.
    /// </summary>
    public bool IsRedisEnabled => FeatureManager.IsEnabledAsync(FeatureFlagConstants.Cache).Result;

    /// <summary>
    /// Initializes a new instance of the <see cref="StoreBase"/> class.
    /// </summary>
    /// <param name="services">The service provider used to resolve dependencies.</param>
    protected StoreBase(IServiceProvider services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }
}

