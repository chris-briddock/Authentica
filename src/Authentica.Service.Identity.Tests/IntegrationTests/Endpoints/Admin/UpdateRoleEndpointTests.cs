using Api.Constants;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

public class UpdateRoleEndpointTests
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
    public async Task UpdateRole_Returns204NoContent_WhenUpdateIsSuccessful()
    {
        // Arrange
        var client = _fixture.CreateAuthenticatedClient();

        var request = new UpdateRoleRequest
        {
            CurrentName = "Test",
            NewName = "UpdatedRoleName"
        };

        // Act
        var response = await client.PutAsJsonAsync($"api/v1/{Routes.Admin.Roles.Update}", request);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

}