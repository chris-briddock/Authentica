using Microsoft.ApplicationInsights.AspNetCore;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;

namespace Authentica.Common;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Azure Application Insights, if enabled.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which services will be added.</param>
    /// <returns>The modified <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddAzureAppInsights(this IServiceCollection services)
    {
        var featureManager = services.BuildServiceProvider()
                                     .GetService<IFeatureManager>()!;

        if (featureManager.IsEnabledAsync(FeatureFlagConstants.AzApplicationInsights).Result)
        {
            services.AddApplicationInsightsTelemetry();
            services.ConfigureTelemetryModule<RequestTrackingTelemetryModule>(
                (module, options) => { module.CollectionOptions.InjectResponseHeaders = true; });
            services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>(
                (module, options) =>
                {
                    module.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.ServiceBus");
                });
            services.AddApplicationInsightsKubernetesEnricher();
        }
        return services;
    }
}
