using Application.Contracts;

namespace Authentica.Service.Identity.Tests.Mocks;

public class UserReadStoreMock : Mock<IUserReadStore>, IMockBase<Mock<IUserReadStore>>
{
    public Mock<IUserReadStore> Mock()
    {
        return this;
    }
}
