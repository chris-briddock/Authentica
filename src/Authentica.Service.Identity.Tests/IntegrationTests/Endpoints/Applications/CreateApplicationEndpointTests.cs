using Api.Constants;
using System.Text;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

public class CreateApplicationEndpointTests
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
    public async Task CreateApplication_Returns201Created_WhenSuccessful()
    {
        var content = new CreateApplicationRequest()
        {
            Name = "Test App",
            CallbackUri = "https://localhost:7256/callback"
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");

        using var sutClient = _fixture.CreateAuthenticatedClient();

        using var sut = await sutClient.PostAsync($"api/v1/{Routes.Applications.Create}", jsonContent);

        var errorContent = await sut.Content.ReadAsStringAsync();

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }

    [Test]
    public async Task CreateApplication_Returns400BadRequest_WhenApplicationExists()
    {
        var content = new CreateApplicationRequest()
        {
            Name = "Default Test Application",
            CallbackUri = "https://localhost:7256/callback"
        };
        using var sutClient = _fixture.CreateAuthenticatedClient();

        var jsonContent = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");

        using var sut = await sutClient.PostAsync($"api/v1/{Routes.Applications.Create}", jsonContent);

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}
