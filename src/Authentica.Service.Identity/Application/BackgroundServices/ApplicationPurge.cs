using Application.Contracts;
using Application.Exceptions;
using Domain.Aggregates.Identity;
using ITimer = Application.Contracts.ITimer;

namespace Application.BackgroundServices;

/// <summary>
/// This background service deletes old user account marked as deleted after seven years.
/// </summary>
public class ApplicationPurge : BackgroundService
{
    /// <summary>
    /// A factory for creating instances of <see cref="IServiceScope"/>
    /// </summary>
    public IServiceScopeFactory ServiceScopeFactory { get; }
    /// <summary>
    /// The application logger.
    /// </summary>
    public ILogger<ApplicationPurge> Logger { get; }

    /// <summary>
    /// A periodic timer.
    /// </summary>
    public readonly ITimer _timer;

    /// <summary>
    /// Initializes a new instance of <see cref="ApplicationPurge"/>
    /// </summary>
    /// <param name="serviceScopeFactory">A factory for creating instances of <see cref="IServiceScope"/></param>
    /// <param name="logger">The application logger.</param>
    /// <param name="timer"></param>
    public ApplicationPurge(IServiceScopeFactory serviceScopeFactory,
                            ILogger<ApplicationPurge> logger,
                            ITimer timer)
    {
        ServiceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _timer = timer ?? throw new ArgumentNullException(nameof(timer));
    }
    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = ServiceScopeFactory.CreateScope();
        while (!stoppingToken.IsCancellationRequested &&
               await _timer.WaitForNextTickAsync(stoppingToken))
        {
            Logger.LogInformation("Executing {methodName}", nameof(ApplicationPurge));

            var sharedStore = scope.ServiceProvider.GetRequiredService<ISharedStore>();

            var result = await sharedStore.PurgeEntriesAsync<ClientApplication>(stoppingToken);

            if (result.Errors.Any())
                throw new PurgeFailureException(result.Errors.First().Description);
        }
    }
}