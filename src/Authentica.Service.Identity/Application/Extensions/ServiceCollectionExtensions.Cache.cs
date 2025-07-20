using Common.Constants;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.FeatureManagement;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Backplane.StackExchangeRedis;

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

        services.AddDistributedMemoryCache();

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
    /// <summary>
    /// Add the required services for fusion/hybrid cache.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which services will be added.</param>
    /// <param name="configuration">The application's configuration.</param>
    /// <returns>The modified <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddHybridCache(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Redis");
        services.AddFusionCacheStackExchangeRedisBackplane();
        services.AddFusionCache()
                .WithDefaultEntryOptions(opt =>
                {
                    opt.Duration = TimeSpan.FromMinutes(5);
                    opt.SetDistributedCacheFailSafeOptions(TimeSpan.FromMinutes(10));
                    opt.SetFailSafe(true, TimeSpan.FromMinutes(10));
                })
                .WithSystemTextJsonSerializer()
                .WithDistributedCache(new RedisCache(new RedisCacheOptions() { Configuration = connectionString }))
                .WithBackplane(new RedisBackplane(new RedisBackplaneOptions { Configuration = connectionString }))
                .AsKeyedService("cache");

        return services;
    }
}