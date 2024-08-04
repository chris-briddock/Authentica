using Api.Constants;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;


public class TwoFactorRecoveryCodesEndpointTests
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
    public async Task TwoFactorRecoveryCodes_Returns200OK_WhenRecoveryCodesAreGeneratedSuccessfully()
    {
        var client = _fixture.CreateAuthenticatedClient();

        var userReadStoreMock = new UserReadStoreMock();

        userReadStoreMock.Setup(x => x.GetUserByEmailAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(UserStoreResult.Success(new User() { Email = "twofactorTest@default.com", TwoFactorEnabled = true}));

        using var sut = await client.GetAsync($"api/v1/{Routes.Users.TwoFactorRecoveryCodes}");

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

}