using Authentica.Common;
using ChristopherBriddock.AspNetCore.Extensions;
using Microsoft.FeatureManagement;

namespace Application.Extensions;

/// <summary>
/// Health check related extension methods.
/// </summary>
public static class HealthCheckExtensions
{
    /// <summary>
    /// Adds health checks for azure application insights.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to healh checks will be added.</param>
    /// <returns>The modified <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddAzureApplicationInsightsHealthChecks(this IServiceCollection services)
    {
        var featureManager = services.BuildServiceProvider()
                                     .GetRequiredService<IFeatureManager>();

        var configuration = services.BuildServiceProvider()
                                    .GetRequiredService<IConfiguration>();

        if (featureManager.IsEnabledAsync(FeatureFlagConstants.AzApplicationInsights).Result)
        {
            var key = configuration["ApplicationInsights:InstrumentationKey"]!;
            services.AddHealthChecks().AddAzureApplicationInsights(key);
        }

        return services;
    }
}
