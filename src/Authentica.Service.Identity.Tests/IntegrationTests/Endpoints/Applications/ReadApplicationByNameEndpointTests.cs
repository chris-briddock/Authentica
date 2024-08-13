using Api.Constants;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

public class ReadApplicationByNameEndpointTests
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
    public async Task ReadApplicationByName_Returns200Ok_WhenRequestIsValid()
    {
        using var sutClient = _fixture.CreateAuthenticatedClient();

        using var response = await sutClient.GetAsync($"api/v1/{Routes.Applications.ReadByName}/?name=Default%20Test%20Application");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task ReadApplicationByName_Returns400BadRequest_WhenApplicationIsNotFound()
    {
        using var sutClient = _fixture.CreateAuthenticatedClient();

        using var response = await sutClient.GetAsync($"api/v1/{Routes.Applications.ReadByName}/?Name=Application");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}
