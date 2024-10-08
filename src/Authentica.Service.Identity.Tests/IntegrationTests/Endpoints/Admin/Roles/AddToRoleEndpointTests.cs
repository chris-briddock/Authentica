using Api.Constants;
using Persistence.Seed;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

public class AddToRoleEndpointTests
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
    public async Task AddToRole_Returns200Ok_WhenSuccessfullyAddedUserToRole()
    {
        var client = _fixture.CreateAuthenticatedClient();

        var request = new AddToRoleRequest()
        {
            Email = Seed.Test.AdminEmail,
            Role = "Test"
        };

        var sut = await client.PutAsJsonAsync($"api/v1/{Routes.Admin.Roles.Add}", request);

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task AddToRole_Returns400BadRequest_WhenUnsuccessful()
    {
        var userManagerMock = new UserManagerMock<User>().Mock();

        userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(new User());
        userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed());

        var client = _fixture.CreateAuthenticatedClient(x => x.Replace(new ServiceDescriptor(typeof(UserManager<User>), userManagerMock.Object)));

        var request = new AddToRoleRequest()
        {
            Email = "test@test.com",
            Role = "Test"
        };

        var sut = await client.PutAsJsonAsync($"api/v1/{Routes.Admin.Roles.Add}", request);

        var errorContent = await sut.Content.ReadAsStringAsync();

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}
