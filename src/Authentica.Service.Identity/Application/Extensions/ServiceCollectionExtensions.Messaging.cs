using Application.Contracts;
using Application.Publishers;
using Authentica.Common;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.FeatureManagement;

namespace Application.Extensions;

public static partial class ServiceCollectionExtensions 
{
    /// <summary>
    /// Adds publisher messaging for rabbitmq or azure service bus.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which services will be added.</param>
    /// <returns>The modified <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddPublisherMessaging(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider()
                                    .GetService<IConfiguration>()!;

        var featureManager = services.BuildServiceProvider()
                                     .GetService<IFeatureManager>()!;

        var rabbitMqEnabled = featureManager.IsEnabledAsync(FeatureFlagConstants.RabbitMq).Result;

        var azServiceBusEnabled = featureManager.IsEnabledAsync(FeatureFlagConstants.AzServiceBus).Result;

        if (azServiceBusEnabled)
        {
            services.AddMassTransit(mt =>
            {
                mt.UsingAzureServiceBus((context, config) =>
                {
                    config.Host(configuration["ConnectionStrings:AzureServiceBus"]);
                    config.ConfigureEndpoints(context);
                });
            });
        }
        if (rabbitMqEnabled)
        {
            services.AddMassTransit(mt =>
            {
                mt.SetKebabCaseEndpointNameFormatter();

                mt.UsingRabbitMq((context, config) =>
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

        if (rabbitMqEnabled || azServiceBusEnabled)
        {
            services.TryAddTransient<IEmailPublisher, EmailPublisher>();
        }
        else
        {
            services.TryAddTransient<IEmailPublisher, NullEmailPublisher>();
        }

        return services;
    }
}