using Api.Constants;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

public class CreateRoleEndpointTests
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
    public async Task CreateRoleEndpoint_Returns201Created_WhenSuccessful()
    {
        var client = _fixture.CreateAuthenticatedClient();

        var request = new CreateRoleRequest()
        {
            Name = "Integration"
        };

        var sut = await client.PostAsJsonAsync($"api/v1/{Routes.Admin.Roles.Create}", request);

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }
}