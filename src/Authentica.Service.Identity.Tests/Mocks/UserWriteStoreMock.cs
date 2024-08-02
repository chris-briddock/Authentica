using Application.Contracts;

namespace Authentica.Service.Identity.Tests.Mocks;

public class UserWriteStoreMock : Mock<IUserWriteStore>, IMockBase<Mock<IUserWriteStore>>
{
    public Mock<IUserWriteStore> Mock()
    {
        return this;
    }
}
