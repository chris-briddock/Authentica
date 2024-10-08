namespace Authentica.Service.Identity.Tests.Mocks;
public class RoleManagerMock : Mock<RoleManager<Role>>, IMockBase<RoleManagerMock>
{
    public RoleManagerMock Mock()
    {
        return this;
    }
}
