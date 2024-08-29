using Api.Constants;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

public class CreateDeviceCodeEndpointTests
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
    public async Task CreateDeviceCode_Returns200OK_WhenDeviceCodeIsCreatedSuccessfully()
    {
        var client = _fixture.CreateAuthenticatedClient();

        var sut = await client.GetAsync($"api/v1/{Routes.OAuth.Device}");

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}
