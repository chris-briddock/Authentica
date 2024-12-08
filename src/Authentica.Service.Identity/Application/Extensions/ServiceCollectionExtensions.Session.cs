using Authentica.Common;
using Microsoft.FeatureManagement;

namespace Application.Extensions;

public static partial class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds custom session configuration to the application.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddCustomSession(this IServiceCollection services)
    {
        services.AddSession(options =>
        {
            options.Cookie.Name = ".AspNet.Session";
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.IdleTimeout = TimeSpan.FromMinutes(60);
        });

        return services;
    }

    /// <summary>
    /// Add the required services for in-memory and redis services, if redis is enabled in the feature flags.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which services will be added.</param>
    /// <returns>The modified <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddSessionCache(this IServiceCollection services)
    {
        IConfiguration configuration = services
                                      .BuildServiceProvider()
                                      .GetService<IConfiguration>()!;
        IFeatureManager featureManager = services
                                        .BuildServiceProvider()
                                        .GetService<IFeatureManager>()!;

        services.AddDistributedMemoryCache();

        if (!featureManager.IsEnabledAsync(FeatureFlagConstants.Cache).Result)
            return services;

        services.AddStackExchangeRedisCache(opt =>
        {
            opt.Configuration = configuration.GetConnectionString("Redis");
        });

        return services;
    }
}