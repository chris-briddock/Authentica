using Api.Constants;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

public class MultiFactorManageAuthenticatorEndpointTests
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

    [Test, Order(1)]
    public async Task MultiFactorManageAuthenticator_Returns400BadRequest_WhenSuccessfullyEnabled()
    {
        var client = _fixture.CreateAuthenticatedClient();

        using var sut = await client.PostAsync($"api/v1/{Routes.Users.MultiFactorManageAuthenticator}?is_enabled=true", null!);

         Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test, Order(2)]
    public async Task MultiFactorManageAuthenticator_Returns200OK_WhenSuccessfullyEnabled()
    {
        var client = _fixture.CreateAuthenticatedClient();

        using var enableTwoFactor = await client.PostAsync($"api/v1/{Routes.Users.MultiFactorManage}?is_enabled=true", null!);

        using var sut = await client.PostAsync($"api/v1/{Routes.Users.MultiFactorManageAuthenticator}?is_enabled=true", null!);

         Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}