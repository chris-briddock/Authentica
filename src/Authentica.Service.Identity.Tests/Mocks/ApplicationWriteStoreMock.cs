using Application.Contracts;

namespace Authentica.Service.Identity.Tests.Mocks;

public class ApplicationWriteStoreMock : Mock<IApplicationWriteStore>, IMockBase<Mock<IApplicationWriteStore>>
{
    public Mock<IApplicationWriteStore> Mock()
    {
        return this;
    }
}
