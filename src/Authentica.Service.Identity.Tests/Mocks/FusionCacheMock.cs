using ZiggyCreatures.Caching.Fusion;

namespace Authentica.Service.Identity.Tests.Mocks;

public class FusionCacheMock : Mock<IFusionCache>, IMockBase<FusionCacheMock>
{
    public FusionCacheMock Mock()
    {
        return this;
    }
}
