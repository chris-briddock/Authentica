using Api.Constants;
using Persistence.Seed;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

public class DisableMultiFactorEndpointTests
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
    public async Task DisableMultiFactor_Returns200OK_WhenSuccessful()
    {
        var request = new DisableMultiFactorRequest()
        {
            Email = Seed.Test.MultiFactorUserEmail
        }; 

        var client = _fixture.CreateAuthenticatedClient();

        var sut = await client.PostAsJsonAsync($"api/v1/{Routes.Admin.DisableMultiFactor}", request);

        sut.EnsureSuccessStatusCode();

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task DisableMultiFactor_Returns400BadRequest_WhenUserIsNotFound()
    {
        var request = new DisableMultiFactorRequest()
        {
            Email = "test@test.com"
        };

        var client = _fixture.CreateAuthenticatedClient();

        var sut = await client.PostAsJsonAsync($"api/v1/{Routes.Admin.DisableMultiFactor}", request);

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}
