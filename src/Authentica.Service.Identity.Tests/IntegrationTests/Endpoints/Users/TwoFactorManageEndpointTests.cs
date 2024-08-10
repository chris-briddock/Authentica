using Api.Constants;
using Application.Contracts;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

public class TwoFactorManageEndpointTests
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
    public async Task TwoFactorManage_Returns204NoContent_WhenTwoFactorIsEnabledSuccessfully()
    {
        var client = _fixture.CreateAuthenticatedClient();

        using var sut = await client.PostAsync($"api/v1/{Routes.Users.TwoFactorManage}?is_enabled=true", null!);

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task TwoFactorManage_ReturnsBadRequest_WhenUserIsNotFound()
    {
        var readStoreMock = new UserReadStoreMock();

        readStoreMock.Setup(x => x.GetUserByEmailAsync(It.IsAny<ClaimsPrincipal>(),
                                                   It.IsAny<CancellationToken>()))
        .ReturnsAsync(UserStoreResult.Failed());
        
        var client = _fixture.CreateAuthenticatedClient(x => x.Replace(new ServiceDescriptor(typeof(IUserReadStore), readStoreMock.Object)));

        using var sut = await client.PostAsync($"api/v1/{Routes.Users.TwoFactorManage}?is_enabled=true", null!);

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}
