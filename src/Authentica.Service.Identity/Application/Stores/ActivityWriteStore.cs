using System.Text.Json;
using Application.Contracts;
using Application.Redactors;
using Domain.Aggregates.Identity;
using Domain.Constants;

namespace Application.Stores;

/// <summary>
/// Provides write operations to the activity log.
/// </summary>
public sealed class ActivityWriteStore : StoreBase, IActivityWriteStore
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ActivityReadStore"/> class.
    /// </summary>
    /// <param name="services">The service provider to retrieve required services for the write store operations.</param>
    /// <remarks>
    /// This constructor initializes the <see cref="ActivityReadStore"/> instance by calling the base constructor with the provided service provider.
    /// </remarks>
    public ActivityWriteStore(IServiceProvider services) : base(services) {}

    /// <inheritdoc/>
    public async Task SaveActivityAsync<T>(T activity) where T : class
    {
        var redactedEvent = ActivityDataRedactor.RedactSensitiveData(activity);
        var eventData = JsonSerializer.Serialize(redactedEvent);

        Activity record = new()
        {
            ActivityType = activity.GetType().Name,
            Data = eventData,
            SequenceId = HttpContext.Session.GetString(SessionConstants.SequenceId)!
        };

        await DbContext.Activities.AddAsync(record);
        await DbContext.SaveChangesAsync();
    }
}
