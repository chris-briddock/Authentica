using Api.Constants;
using Persistence.Seed;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

public class AdminResetPasswordEndpointTests
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
    public async Task ResetPassword_Returns204NoContent_WhenResetIsSuccessful()
    {
        var client = _fixture.CreateAuthenticatedClient();

        var request = new AdminPasswordResetRequest()
        {
            Email = Seed.Test.AdminEmail,
            Password = "69{}'#ddksdjcdscdDs"
        };
        var sut = await client.PostAsJsonAsync($"api/v1/{Routes.Admin.ResetPassword}", request);

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }
    [Test]
    public async Task ResetPassword_Returns400BadRequest_WhenUserIsNotFound()
    {
        var client = _fixture.CreateAuthenticatedClient();

        var request = new AdminPasswordResetRequest()
        {
            Email = "test@test.com",
            Password = "69{}'#ddksdjcdscdDs"
        };
        var sut = await client.PostAsJsonAsync($"api/v1/{Routes.Admin.ResetPassword}", request);

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}