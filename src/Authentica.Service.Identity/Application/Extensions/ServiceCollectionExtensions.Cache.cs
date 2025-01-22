using Authentica.Common;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.FeatureManagement;

namespace Application.Extensions;

public static partial class ServiceCollectionExtensions
{
    /// <summary>
    /// Add the required services for in-memory cache.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which services will be added.</param>
    /// <returns>The modified <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddInMemoryCache(this IServiceCollection services)
    {
        services.AddMemoryCache();

        services.Configure<MemoryCacheEntryOptions>(options =>
        {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            options.SlidingExpiration = TimeSpan.FromMinutes(5);
        });

        return services;
    }   
    /// <summary>
    /// Add the required services for in-memory and redis services, if redis is enabled in the feature flags.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which services will be added.</param>
    /// <param name="configuration">The application's configuration.</param>
    /// <returns>The modified <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddDistributedCache(this IServiceCollection services,
                                                         IConfiguration configuration)
    {
        IFeatureManager featureManager = services
                                        .BuildServiceProvider()
                                        .GetRequiredService<IFeatureManager>();

        services.AddDistributedMemoryCache();

        if (!featureManager.IsEnabledAsync(FeatureFlagConstants.Cache).Result)
            return services;

        services.AddStackExchangeRedisCache(opt =>
        {
            opt.Configuration = configuration.GetConnectionString("Redis");
        });

        services.Configure<DistributedCacheEntryOptions>(options =>
        {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            options.SlidingExpiration = TimeSpan.FromMinutes(5);
        });

        return services;
    }
}