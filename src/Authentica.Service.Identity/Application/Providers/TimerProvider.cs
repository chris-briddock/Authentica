using ITimer = Application.Contracts.ITimer;

namespace Application.Providers;

/// <summary>
/// Provides a timer that ticks at regular intervals and allows waiting for the next tick.
/// </summary>
public sealed class TimerProvider : ITimer, IDisposable
{
    private readonly PeriodicTimer _timer;

    /// <summary>
    /// Initializes a new instance of the <see cref="TimerProvider"/> class.
    /// The timer is set to tick every day.
    /// </summary>
    public TimerProvider()
    {
        _timer = new PeriodicTimer(TimeSpan.FromDays(1));
    }

    /// <summary>
    /// Waits asynchronously for the next tick of the timer.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the wait.</param>
    /// <returns>A task that represents the asynchronous wait operation. The task result is true if the next tick occurred; false if the wait was canceled.</returns>
    public async Task<bool> WaitForNextTickAsync(CancellationToken cancellationToken)
    {
        return await _timer.WaitForNextTickAsync(cancellationToken);
    }

    /// <summary>
    /// Releases all resources used by the current instance of the <see cref="TimerProvider"/> class.
    /// </summary>
    public void Dispose()
    {
        _timer.Dispose();
        GC.SuppressFinalize(this);
    }
}