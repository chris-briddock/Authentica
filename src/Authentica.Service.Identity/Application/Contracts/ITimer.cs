namespace Application.Contracts;

public interface ITimer
{
    Task<bool> WaitForNextTickAsync(CancellationToken cancellationToken);
}
