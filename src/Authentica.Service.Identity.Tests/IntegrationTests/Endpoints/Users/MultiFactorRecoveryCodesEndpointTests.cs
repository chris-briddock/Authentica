using Api.Constants;
using Persistence.Seed;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;


public class MultiFactorRecoveryCodesEndpointTests
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
    public async Task MultiFactorRecoveryCodes_Returns200OK_WhenRecoveryCodesAreGeneratedSuccessfully()
    {
        var client = _fixture.CreateAuthenticatedClient();

        var userReadStoreMock = new UserReadStoreMock();

        userReadStoreMock.Setup(x => x.GetUserByEmailAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(UserStoreResult.Success(new User() { Email = Seed.Test.MultiFactorUserEmail, TwoFactorEnabled = true}));

        using var sut = await client.GetAsync($"api/v1/{Routes.Users.MultiFactorRecoveryCodes}");

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

}