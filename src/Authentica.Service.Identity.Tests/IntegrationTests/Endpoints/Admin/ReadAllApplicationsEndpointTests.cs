using Api.Constants;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

public class ReadAllApplicationsEndpointTests
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
    public async Task GetAllApplications_Returns200OK_WhenSuccessful()
    {
        var client = _fixture.CreateAuthenticatedClient();

        var sut = await client.GetAsync($"api/v1/{Routes.Admin.ReadAllApplications}");

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

}