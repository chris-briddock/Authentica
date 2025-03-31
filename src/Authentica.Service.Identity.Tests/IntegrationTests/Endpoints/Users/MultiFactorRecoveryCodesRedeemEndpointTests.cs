using Persistence.Seed;
using System.Text;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

public class MultiFactorRecoveryCodesRedeemEndpointTests
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
    public async Task MultiFactorRecoveryReedem_Returns200OK_WhenRedeemIsSuccessful()
    {
        var userWriteStore = new UserWriteStoreMock();
        userWriteStore.Setup(x => x.RedeemMultiFactorRecoveryCodeAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(UserStoreResult.Success());

        using var client = _fixture.CreateAuthenticatedClient(x =>
        {
            x.Replace(new ServiceDescriptor(typeof(IUserWriteStore), userWriteStore.Object));
        });

        var request = new MultiFactorRecoveryCodeRedeemRequest()
        {
            Email = Seed.Test.MultiFactorUserEmail,
            Code = "888888"
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var sut = await client.PostAsync($"{Routes.Users.MultiFactorAuthentication.RecoveryCodes}", jsonContent);

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task MultiFactorRecoveryReedem_Returns400BadRequest_WhenRedeemIsUnsuccessful()
    {
        var userWriteStore = new UserWriteStoreMock();
        userWriteStore.Setup(x => x.RedeemMultiFactorRecoveryCodeAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(UserStoreResult.Failed());

        using var client = _fixture.CreateAuthenticatedClient(x =>
        {
            x.Replace(new ServiceDescriptor(typeof(IUserWriteStore), userWriteStore.Object));
        });

        var request = new MultiFactorRecoveryCodeRedeemRequest()
        {
            Email = Seed.Test.MultiFactorUserEmail,
            Code = "888888"
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var sut = await client.PostAsync($"{Routes.Users.MultiFactorAuthentication.RecoveryCodes}", jsonContent);

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}