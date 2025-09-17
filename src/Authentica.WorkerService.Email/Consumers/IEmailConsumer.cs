using MassTransit;

namespace Authentica.WorkerService.Email.Consumers;

/// <summary>
/// Interface for email consumers that handle specific events.
/// </summary>
/// <typeparam name="TEvent">The type of event to consume.</typeparam>
public interface IEmailConsumer<in TEvent> : IConsumer<TEvent>
    where TEvent : class
{
}
