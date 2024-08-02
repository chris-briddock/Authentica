using Microsoft.FeatureManagement;

namespace Authentica.Service.Identity.Tests.Mocks;

public class FeatureManagerMock : Mock<IFeatureManager>, IMockBase<FeatureManagerMock>
{
    public FeatureManagerMock Mock()
    {
        return this;
    }
}
