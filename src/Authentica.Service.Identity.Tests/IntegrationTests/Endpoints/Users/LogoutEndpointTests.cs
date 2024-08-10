namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

public class LogoutEndpointTests
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
    public async Task Logout_Returns204NoContent_WhenSignOutIsSuccessful()
    {
        var client = _fixture.CreateAuthenticatedClient();

        var sut = await client.PostAsync($"api/v1/users/logout", null!);

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }
}
