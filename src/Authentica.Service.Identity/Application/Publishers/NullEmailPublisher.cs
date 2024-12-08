using Application.Contracts;
using Authentica.Common;
using System.Diagnostics.CodeAnalysis;
namespace Application.Publishers;

/// <summary>
/// This is a null implementation of <see cref="IEmailPublisher"/>
/// Allows for the system to be tested without needing a message queue available.
/// This will mean that the system will no longer be able to send emails for registration confirmation,
/// mfa authentication and forgot password, but this is only when the Feature Flag for RabbitMq or 
/// Azure Service Bus is disabled.
/// </summary>
public class NullEmailPublisher : IEmailPublisher
{
    /// <inheritdoc/>
    [ExcludeFromCodeCoverage]
    public async Task Publish(EmailMessage emailMessage, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}
