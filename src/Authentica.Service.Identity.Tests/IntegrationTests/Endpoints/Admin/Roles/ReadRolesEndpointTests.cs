using Api.Constants;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

public class ReadRolesEndpointTests
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
    public async Task GetRoles_Returns200Ok_WithListOfRoles()
    {
        // Arrange
        var client = _fixture.CreateAuthenticatedClient();

        // Act
        var response = await client.GetAsync($"api/v1/{Routes.Admin.Roles.Read}");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var roles = await response.Content.ReadFromJsonAsync<List<Role>>();
        Assert.That(roles, Is.Not.Null);
        Assert.That(roles, Is.Not.Empty);
    }
}
