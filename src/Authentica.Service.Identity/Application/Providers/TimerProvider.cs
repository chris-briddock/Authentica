using ITimer = Application.Contracts.ITimer;

namespace Application.Providers;

public class TimerProvider : ITimer, IDisposable
{
    private readonly PeriodicTimer _timer;

    public TimerProvider()
    {
        _timer = new PeriodicTimer(TimeSpan.FromDays(1));
    }

    public async Task<bool> WaitForNextTickAsync(CancellationToken cancellationToken)
    {
        return await _timer.WaitForNextTickAsync(cancellationToken);
    }

    public void Dispose()
    {
        _timer.Dispose();
    }
}