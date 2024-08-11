using Api.Constants;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

public class DisableTwoFactorEndpointTests
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
    public async Task DisableTwoFactor_Returns200OK_WhenSuccessful()
    {
        var request = new DisableTwoFactorRequest()
        {
            Email = "twofactorTest@default.com"
        };

        var client = _fixture.CreateAuthenticatedClient();

        var sut = await client.PostAsJsonAsync($"api/v1/{Routes.Admin.DisableTwoFactor}", request);

        sut.EnsureSuccessStatusCode();

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task DisableTwoFactor_Returns400BadRequest_WhenUserIsNotFound()
    {
        var request = new DisableTwoFactorRequest()
        {
            Email = "twofactorTest@default1.com"
        };

        var client = _fixture.CreateAuthenticatedClient();

        var sut = await client.PostAsJsonAsync($"api/v1/{Routes.Admin.DisableTwoFactor}", request);

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}
