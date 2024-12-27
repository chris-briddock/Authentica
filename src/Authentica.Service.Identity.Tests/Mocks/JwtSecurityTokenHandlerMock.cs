namespace Authentica.Service.Identity.Tests.Mocks;

public class JwtSecurityTokenHandlerMock : Mock<JwtSecurityTokenHandler>, IMockBase<JwtSecurityTokenHandlerMock>
{
    public JwtSecurityTokenHandlerMock Mock()
    {
        return this;
    }
}