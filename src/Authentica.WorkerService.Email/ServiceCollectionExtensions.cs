using Authentica.WorkerService.Email.Consumers;
using Authentica.WorkerService.Email.Services;
using Common.Constants;
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

        // Register the EmailService
        services.AddScoped<EmailService>();

        if (featureManager.IsEnabledAsync(FeatureFlagConstants.AzServiceBus).Result)
        {
            services.AddMassTransit(mt =>
            {
                mt.SetKebabCaseEndpointNameFormatter();

                // Register all consumers
                RegisterConsumers(mt);

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

                // Register all consumers
                RegisterConsumers(x);

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

    /// <summary>
    /// Registers all the event consumers with MassTransit.
    /// </summary>
    /// <param name="configurator">The MassTransit registration configurator.</param>
    private static void RegisterConsumers(IBusRegistrationConfigurator configurator)
    {
        // Email code consumers
        configurator.AddConsumer<MfaEmailCodeSentConsumer>();
        configurator.AddConsumer<UserEmailConfirmationCodeSentConsumer>();
        configurator.AddConsumer<UserPasswordResetRequestedConsumer>();
        configurator.AddConsumer<UserUpdateEmailCodeSentConsumer>();
        configurator.AddConsumer<UpdatePhoneNumberCodeSentConsumer>();

        // Notification consumers
        configurator.AddConsumer<UserRegisteredConsumer>();
        configurator.AddConsumer<UserEmailConfirmedConsumer>();
        configurator.AddConsumer<UserPasswordResetConsumer>();
        configurator.AddConsumer<MfaEnabledConsumer>();
        configurator.AddConsumer<MfaDisabledConsumer>();
    }
}
