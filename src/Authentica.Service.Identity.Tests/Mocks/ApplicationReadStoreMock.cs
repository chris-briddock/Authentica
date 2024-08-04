using Application.Contracts;

namespace Authentica.Service.Identity.Tests.Mocks;

public class ApplicationReadStoreMock : Mock<IApplicationReadStore>, IMockBase<Mock<IApplicationReadStore>>
{
    public Mock<IApplicationReadStore> Mock()
    {
        return this;
    }
}
