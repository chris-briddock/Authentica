namespace Authentica.Service.Identity.Tests.Mocks;

public class HttpContextAccessorMock : Mock<IHttpContextAccessor>, IMockBase<HttpContextAccessorMock>
{
    public HttpContextAccessorMock Mock()
    {
        return this;
    }
}