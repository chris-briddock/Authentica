using ITimer = Application.Contracts.ITimer;

namespace Authentica.Service.Identity.Tests.Mocks;

public class TimerMock : Mock<ITimer>, IMockBase<Mock<ITimer>>
{
    public Mock<ITimer> Mock()
    {
        return this;
    }
}