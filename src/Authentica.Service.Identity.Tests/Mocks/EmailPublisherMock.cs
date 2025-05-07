using Domain.Contracts;

namespace Authentica.Service.Identity.Tests.Mocks;

public class EmailPublisherMock : Mock<IPublisher>, IMockBase<EmailPublisherMock>
{
    public EmailPublisherMock Mock()
    {
        return this;
    }
}
