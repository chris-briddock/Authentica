using Api.Constants;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

public class DeleteRoleEndpointTests
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
    public async Task DeleteRole_Returns204NoContent_WhenSoftDeleteIsSuccessful()
    {
        // Arrange
        var client = _fixture.CreateAuthenticatedClient();

        var request = new DeleteRoleRequest()
        {
            Name = "Test"
        };

        // Act
        var sut = await client.DeleteAsync($"api/v1/{Routes.Admin.Roles.Delete}?name={request.Name}");

        // Assert
        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task DeleteRole_Returns400BadRequest_WhenRoleIsNotFound()
    {
         // Arrange
        var client = _fixture.CreateAuthenticatedClient();

        var request = new DeleteRoleRequest()
        {
            Name = "NotFound"
        };

        // Act
        var sut = await client.DeleteAsync($"api/v1/{Routes.Admin.Roles.Delete}?name={request.Name}");

        // Assert
        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}