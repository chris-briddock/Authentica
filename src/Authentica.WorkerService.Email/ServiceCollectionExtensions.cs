using Authentica.Common;
using MassTransit;
using Microsoft.FeatureManagement;

namespace Authentica.WorkerService.Email;

internal static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds consumer messages for rabbitmq or azure service bus.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which services will be added.</param>
    /// <returns>The modified <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddConsumerMessaging(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

        var featureManager = services.BuildServiceProvider().GetRequiredService<IFeatureManager>();

        if (featureManager.IsEnabledAsync(FeatureFlagConstants.AzServiceBus).Result)
        {
            services.AddMassTransit(mt =>
            {
                mt.SetKebabCaseEndpointNameFormatter();

                mt.AddConsumer<Worker>();

                mt.UsingAzureServiceBus((context, config) =>
                {
                    config.Host(configuration["ConnectionStrings:AzureServiceBus"]);
                    config.ConfigureEndpoints(context);
                });
            });
        }
        if (featureManager.IsEnabledAsync(FeatureFlagConstants.RabbitMq).Result)
        {

            services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.AddConsumer<Worker>();

            x.UsingRabbitMq((context, config) =>
            {

                config.Host(configuration["RabbitMQ:Hostname"], "/", r =>
                {
                    r.Username(configuration["RabbitMQ:Username"]!);
                    r.Password(configuration["RabbitMQ:Password"]!);
                });
                config.ConfigureEndpoints(context);
            });
        });

        }
        return services;
    }
}
