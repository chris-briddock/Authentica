using System.Text;
using Api.Constants;
using Persistence.Seed;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

public class LoginEndpointTests
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
    public async Task LoginEndpoint_Returns200OK_WhenLoginIsSuccessful()
    {
        var client = _fixture.WebApplicationFactory.CreateClient();

        var request = new LoginRequest()
        {
            Email = Seed.Test.AuthorizeUserEmail,
            Password = "7XAl@Dg()[=8rV;[wD[:GY$yw:$ltHAuaf!UQ`",
            RememberMe = true
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        
        var result = await client.PostAsync($"/api/v1/{Routes.Users.Login}", jsonContent);
        
        var cookies = result.Headers.GetValues("Set-Cookie");

        bool hasMyCookie = cookies.Any(header => header.Contains(IdentityConstants.ApplicationScheme));

        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(hasMyCookie, Is.EqualTo(true));

    }

    [Test]
    public async Task LoginEndpoint_Returns200OK_WhenMultiFactorIsRequired()
    {
        var client = _fixture.WebApplicationFactory.CreateClient();

        var request = new LoginRequest()
        {
            Email = Seed.Test.MultiFactorUserEmail,
            Password = "Ar*P`w8R.WyXb7'UKxh;!-",
            RememberMe = true
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        
        var result = await client.PostAsync($"/api/v1/{Routes.Users.Login}", jsonContent);
        
        var cookies = result.Headers.GetValues("Set-Cookie");

        bool hasMyCookie = cookies.Any(header => header.Contains(IdentityConstants.TwoFactorUserIdScheme));

        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(hasMyCookie, Is.EqualTo(true));

    }

    [Test]
    public async Task LoginEndpoint_Returns401Unauthorized_WhenLoginFails()
    {
        var client = _fixture.WebApplicationFactory.CreateClient();

        var request = new LoginRequest()
        {
            Email = Seed.Test.MultiFactorUserEmail,
            Password = "Ar*P`w8R.WyXb7'UKxh-",
            RememberMe = true
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        
        var result = await client.PostAsync($"/api/v1/{Routes.Users.Login}", jsonContent);
    

        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));

    }

    [Test]
    public async Task LoginEndpoint_Returns400BadRequest_WhenUserIsNotFound()
    {
        var client = _fixture.WebApplicationFactory.CreateClient();

        var request = new LoginRequest()
        {
            Email = "notfound@test.com",
            Password = "Ar*P`w8R.WyXb7'UKxh-",
            RememberMe = true
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        
        var result = await client.PostAsync($"/api/v1/{Routes.Users.Login}", jsonContent);

        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

    }
}

