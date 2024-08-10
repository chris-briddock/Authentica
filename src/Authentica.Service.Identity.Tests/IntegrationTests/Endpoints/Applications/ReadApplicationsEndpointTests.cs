using Api.Constants;
using Application.Contracts;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

public class ReadApplicationsEndpointTests
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
    public async Task ReadApplications_Returns200Ok_WhenRequestIsValid()
    {
        using var sutClient = _fixture.CreateAuthenticatedClient();

        using var response = await sutClient.GetAsync($"api/v1/{Routes.Applications.ReadAll}");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
    [Test]
    public async Task ReadApplications_ReturnsBadRequest_WhenUserIsNotFound()
    {
        var userReadStoreMock = new UserReadStoreMock();

        userReadStoreMock.Setup(x => x.GetUserByEmailAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>()));

        using var sutClient = _fixture.CreateAuthenticatedClient(x => 
        {
            x.Replace(new ServiceDescriptor(typeof(IUserReadStore), userReadStoreMock.Object));
        });

        using var response = await sutClient.GetAsync($"api/v1/{Routes.Applications.ReadAll}");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}
