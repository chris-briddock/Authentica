using System.Text;
using Api.Constants;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

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
            Name = "Default Test Application"
        };

        using var sutClient = _fixture.CreateAuthenticatedClient();

        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        using var sut = await sutClient.PutAsync($"api/v1/{Routes.Applications.ApplicationSecrets}", jsonContent);

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}
