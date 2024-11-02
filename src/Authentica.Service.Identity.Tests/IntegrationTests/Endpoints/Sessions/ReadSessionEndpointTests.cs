using Api.Constants;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

public class ReadSessionsEndpointTests
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
    public async Task ReadSessions_Returns200OK_WhenSessionsExist()
    {
        // Arrange
        var client = _fixture.CreateAuthenticatedClient();

        // Act
        using var response = await client.GetAsync($"api/v1/{Routes.Sessions.Name}");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK)); 
    }

    [Test, Order(2)]
    public async Task ReadSessions_Returns401Unauthorized_WhenUserNotAuthenticated()
    {
        // Arrange
        var client = _fixture.WebApplicationFactory.CreateClient();

        // Act
        using var response = await client.GetAsync($"api/v1/{Routes.Sessions.Name}");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }
}