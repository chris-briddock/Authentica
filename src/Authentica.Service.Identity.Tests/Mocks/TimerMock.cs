using Application.Contracts;

namespace Authentica.Service.Identity.Tests.Mocks;

public class TimerMock : Mock<ITimerProvider>, IMockBase<Mock<ITimerProvider>>
{
    public Mock<ITimerProvider> Mock()
    {
        return this;
    }
}