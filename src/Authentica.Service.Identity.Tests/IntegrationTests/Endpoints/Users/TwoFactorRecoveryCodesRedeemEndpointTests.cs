using Api.Constants;
using Application.Contracts;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

public class TwoFactorRecoveryCodesRedeemEndpointTests
{
    private TestFixture<Program> _fixture;

    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        _fixture = new TestFixture<Program>();
        await _fixture.OneTimeSetUpAsync();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _fixture.OneTimeTearDown();
    }

    [Test]
    public async Task TwoFactoryRecoveryReedem_Returns200OK_WhenRedeemIsSuccessful()
    {
        var userWriteStore = new UserWriteStoreMock();
        userWriteStore.Setup(x => x.RedeemTwoFactorRecoveryCodeAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(UserStoreResult.Success());
        
        using var client = _fixture.CreateAuthenticatedClient(x => 
        {
            x.Replace(new ServiceDescriptor(typeof(IUserWriteStore), userWriteStore.Object));
        });

        var request = new TwoFactorRecoveryCodeRedeemRequest()
        {
            Email = "twofactorTest@default.com",
            Code = "888888"
        };

        var sut = await client.PostAsJsonAsync($"api/v1/{Routes.Users.TwoFactorRedeemRecoveryCodes}", request);

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}