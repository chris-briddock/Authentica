using Application.Factories;
using Application.Redactors;
using Application.Results;
using Domain.Aggregates.Identity;
using Domain.Constants;
using Domain.Contracts.Stores;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Application.Stores;

/// <summary>
/// Provides write operations to the activity log.
/// </summary>
public sealed class ActivityWriteStore : StoreBase, IActivityWriteStore
{
    /// <summary>
    /// Gets the Activity DbSet.
    /// </summary>
    private DbSet<Activity> DbSet => DbContext.Set<Activity>();
    /// <summary>
    /// Initializes a new instance of the <see cref="ActivityReadStore"/> class.
    /// </summary>
    /// <param name="services">The service provider to retrieve required services for the write store operations.</param>
    /// <remarks>
    /// This constructor initializes the <see cref="ActivityReadStore"/> instance by calling the base constructor with the provided service provider.
    /// </remarks>
    public ActivityWriteStore(IServiceProvider services) : base(services) { }

    /// <inheritdoc/>
    public async Task<ActivityStoreResult> SaveActivityAsync<T>(T activity) where T : class
    {
        try
        {
            var redactedEvent = ActivityDataRedactor.RedactSensitiveData(activity);
            var eventData = JsonSerializer.Serialize(redactedEvent);

            Activity record = new()
            {
                ActivityType = activity.GetType().Name,
                Data = eventData,
                SequenceId = HttpContext.Session.GetString(SessionConstants.SequenceId)!
            };

            await DbSet.AddAsync(record);
            await DbContext.SaveChangesAsync();

            return ActivityStoreResult.Success();
        }
        catch (Exception ex)
        {
            return ActivityStoreResult.Failed(IdentityErrorFactory.ExceptionOccurred(ex));
        }
    }
}
