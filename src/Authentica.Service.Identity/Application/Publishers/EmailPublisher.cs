using Application.Contracts;
using Authentica.Common;
using MassTransit;
using Microsoft.FeatureManagement;

namespace Application.Publishers;

/// <summary>
/// Publishes a message to the message queue, for confirmation emails, 
/// password reset codes, password reset links and mfa codes.
/// </summary>
public sealed class EmailPublisher : IEmailPublisher
{
    /// <summary>
    /// The application's service provider
    /// </summary>
    public IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Initalizes a new instance of <see cref="EmailPublisher"/>
    /// </summary>
    public EmailPublisher(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public async Task Publish(EmailMessage emailMessage,
                              CancellationToken cancellationToken)
    {

        var bus = ServiceProvider.GetService<IPublishEndpoint>()!;
        var featureManager = ServiceProvider.GetService<IFeatureManager>()!;

        if (await featureManager!.IsEnabledAsync(FeatureFlagConstants.RabbitMq) ||
            await featureManager!.IsEnabledAsync(FeatureFlagConstants.AzServiceBus))
        {
            await bus.Publish(emailMessage, cancellationToken);
        }
    }
}
