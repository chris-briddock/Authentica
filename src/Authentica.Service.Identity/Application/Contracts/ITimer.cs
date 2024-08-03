namespace Application.Contracts;

/// <summary>
/// Defines an contract for a timer that waits for the next tick.
/// </summary>
public interface ITimer
{
    /// <summary>
    /// Asynchronously waits for the next tick of the timer.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the wait operation.</param>
    /// <returns>A task that represents the asynchronous wait operation. The task result is <c>true</c> if the wait completed successfully, <c>false</c> if the wait was canceled.</returns>
    Task<bool> WaitForNextTickAsync(CancellationToken cancellationToken);
}
