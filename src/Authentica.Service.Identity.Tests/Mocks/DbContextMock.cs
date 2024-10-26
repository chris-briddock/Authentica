using Persistence.Contexts;

namespace Authentica.Service.Identity.Tests.Mocks;

public class DbContextMock : Mock<AppDbContext>, IMockBase<Mock<AppDbContext>>
{
    public Mock<AppDbContext> Mock()
    {
        return this;
    }
}