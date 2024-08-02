using System.Collections.Immutable;
using System.Text.Json;
using Application.Contracts;
using Application.Redactors;
using Domain.Aggregates.Identity;
using Domain.Constants;

namespace Application.Stores;

/// <summary>
/// Provides write operations to the event log.
/// </summary>
public sealed class EventStore : StoreBase, IEventStore
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventStore"/> class.
    /// </summary>
    /// <param name="services">The service provider to retrieve required services for the write store operations.</param>
    /// <remarks>
    /// This constructor initializes the <see cref="EventStore"/> instance by calling the base constructor with the provided service provider.
    /// </remarks>
    public EventStore(IServiceProvider services) : base(services)
    {
    }
    /// <inheritdoc/>
    public ImmutableList<Event> GetEventsBySessionId(string sessionId)
    {
        var events = DbContext.Events.Where(x => x.SequenceId == sessionId)
                    .OrderBy(x => x.CreatedOn)
                    .ToImmutableList();
        
        return events;
    }
    /// <inheritdoc/>
    public ImmutableList<Event> GetEventsByTimeStamp(DateTime timeStamp)
    {
        var events = DbContext.Events.Where(x => x.CreatedOn == timeStamp)
                    .OrderBy(x => x.CreatedOn)
                    .ToImmutableList();
        
        return events;
    }
    /// <inheritdoc/>
    public async Task SaveEventAsync<T>(T @event) where T : class
    {
        try
        {
            var redactedEvent = EventDataRedactor.RedactSensitiveData(@event);
            var eventData = JsonSerializer.Serialize(redactedEvent);

            Event record = new()
            {
                EventType = @event.GetType().Name,
                CreatedOn = DateTime.UtcNow,
                Data = eventData,
                SequenceId = HttpContext.Session.GetString(SessionConstants.SequenceId)!
            };

            await DbContext.Events.AddAsync(record);
            await DbContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }
}
