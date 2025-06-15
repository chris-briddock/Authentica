using Application.Publishers;
using Common.Constants;
using Domain.Contracts;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.FeatureManagement;
using Persistence.Contexts;

namespace Application.Extensions;

public static partial class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds publisher messaging for rabbitmq or azure service bus.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which services will be added.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>The modified <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddPublisherMessaging(this IServiceCollection services, IConfiguration configuration)
    {

        var featureManager = services.BuildServiceProvider().GetRequiredService<IFeatureManager>();

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

                mt.AddEntityFrameworkOutbox<AppDbContext>(o =>
                {
                    o.QueryDelay = TimeSpan.FromSeconds(1);
                    o.UseSqlServer();
                    o.UseBusOutbox();
                    o.DuplicateDetectionWindow = TimeSpan.FromMinutes(5);
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

                mt.AddEntityFrameworkOutbox<AppDbContext>(o =>
                {
                    o.QueryDelay = TimeSpan.FromSeconds(1);
                    o.UseSqlServer();
                    o.UseBusOutbox();
                    o.DuplicateDetectionWindow = TimeSpan.FromMinutes(5);
                });
            });
        }

        if (rabbitMqEnabled || azServiceBusEnabled)
        {
            services.TryAddTransient<IPublisher, EmailPublisher>();
        }
        else
        {
            services.TryAddTransient<IPublisher, NullEmailPublisher>();
        }

        return services;
    }
}