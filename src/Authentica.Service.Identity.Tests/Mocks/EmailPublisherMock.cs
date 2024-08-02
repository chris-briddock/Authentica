using Application.Contracts;

namespace Authentica.Service.Identity.Tests.Mocks;

public class EmailPublisherMock : Mock<IEmailPublisher>, IMockBase<EmailPublisherMock>
{
    public EmailPublisherMock Mock()
    {
        return this;
    }
}
