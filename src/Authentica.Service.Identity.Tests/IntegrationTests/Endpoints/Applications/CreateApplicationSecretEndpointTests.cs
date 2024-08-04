using Api.Constants;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

[TestFixture]
public class CreateApplicationSecretEndpointTests
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
    public async Task CreateApplication_Returns201Created_WhenRequestIsValid()
    {
        var request = new CreateApplicationSecretRequest()
        {
            Name = "Default Application"
        };

        using var sutClient = _fixture.CreateAuthenticatedClient();

        using var sut = await sutClient.PutAsJsonAsync($"api/v1/{Routes.Applications.ApplicationSecrets}", request);

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}
