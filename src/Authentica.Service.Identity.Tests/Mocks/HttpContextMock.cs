namespace Authentica.Service.Identity.Tests.Mocks;
public class HttpContextMock : Mock<HttpContext>, IMockBase<HttpContextMock>
{
    public HttpContextMock Mock()
    {
        return this;
    }
}