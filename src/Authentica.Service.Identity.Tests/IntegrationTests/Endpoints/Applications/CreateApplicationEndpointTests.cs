using Api.Constants;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

[TestFixture]
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
    public async Task CreateApplication_Returns201Created_WhenRequestIsValid()
    {
        var request = new CreateApplicationRequest()
        {
            Name = "Test App",
            CallbackUri = "https://localhost:7256/callback",
            RedirectUri = "https://localhost:7256"
        };
        using var sutClient = _fixture.CreateAuthenticatedClient();

        using var sut = await sutClient.PostAsJsonAsync($"api/v1/{Routes.Applications.Create}", request);

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }

    [Test]
    public async Task CreateApplication_Returns400BadRequest_WhenApplicationExists()
    {
        var request = new CreateApplicationRequest()
        {
            Name = "Default Application",
            CallbackUri = "https://localhost:7256/callback",
            RedirectUri = "https://localhost:7256"
        };
        using var sutClient = _fixture.CreateAuthenticatedClient();

        using var sut = await sutClient.PostAsJsonAsync($"api/v1/{Routes.Applications.Create}", request);

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}
