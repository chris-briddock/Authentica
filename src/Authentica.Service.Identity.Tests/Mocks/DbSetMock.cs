using Microsoft.EntityFrameworkCore;

namespace Authentica.Service.Identity.Tests.Mocks;

public class DbSetMock<T> : Mock<DbSet<T>>, IMockBase<Mock<DbSet<T>>> where T : class
{
    public Mock<DbSet<T>> Mock()
    {
        return this;
    }
}