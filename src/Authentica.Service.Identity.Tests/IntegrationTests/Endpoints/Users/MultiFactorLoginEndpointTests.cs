using System.Text;
using Api.Constants;
using Persistence.Seed;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

public class MultiFactorLoginEndpointTests
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
    public async Task MultiFactorLogin_Returns401Unauthorized_WhenMFAIsNotEnabledSuccessfully()
    {
        var signInManagerMock = new SignInManagerMock<User>().Mock();

        signInManagerMock.Setup(x => x.GetTwoFactorAuthenticationUserAsync()).ReturnsAsync(new User());
        signInManagerMock.Setup(x => x.TwoFactorSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(SignInResult.Success);
        
        var client = _fixture.WebApplicationFactory.WithWebHostBuilder(c => 
        {
            c.ConfigureServices(s => 
            {
                s.Replace(new ServiceDescriptor(typeof(SignInManager<User>), signInManagerMock.Object));
            });
        }).CreateClient();

        var request = new MultiFactorLoginRequest()
        {
            Token = "888888" // must be 6 chars.
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        using var sut = await client.PostAsync($"/api/v1/{Routes.Users.MultiFactorLogin}", jsonContent);

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task MultiFactorLogin_Returns400BadRequest_WhenUserIsNotFound()
    {
        var signInManagerMock = new SignInManagerMock<User>().Mock();

        signInManagerMock.Setup(x => x.GetTwoFactorAuthenticationUserAsync());
        signInManagerMock.Setup(x => x.TwoFactorSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(SignInResult.Success);
        
        var client = _fixture.WebApplicationFactory.WithWebHostBuilder(c => 
        {
            c.ConfigureServices(s => 
            {
                s.Replace(new ServiceDescriptor(typeof(SignInManager<User>), signInManagerMock.Object));
            });
        }).CreateClient();

        var request = new MultiFactorLoginRequest()
        {
            Token = "888888" // must be 6 chars.
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        using var sut = await client.PostAsync($"/api/v1/{Routes.Users.MultiFactorLogin}", jsonContent);   

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task MultiFactorLogin_Returns200OK_WhenLoginIsSuccessful()
    {
        var signInManagerMock = new SignInManagerMock<User>().Mock();

        signInManagerMock.Setup(x => x.GetTwoFactorAuthenticationUserAsync()).ReturnsAsync(new User() { Email = Seed.Test.MultiFactorUserEmail, TwoFactorEnabled = true} );
        signInManagerMock.Setup(x => x.TwoFactorSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(SignInResult.Success);
        
        var client = _fixture.WebApplicationFactory.WithWebHostBuilder(c => 
        {
            c.ConfigureServices(s => 
            {
                s.Replace(new ServiceDescriptor(typeof(SignInManager<User>), signInManagerMock.Object));
            });
        }).CreateClient();

        var request = new MultiFactorLoginRequest()
        {
            Token = "888888" // must be 6 chars.
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        using var sut = await client.PostAsync($"/api/v1/{Routes.Users.MultiFactorLogin}", jsonContent);

        var errorResponse = await sut.Content.ReadAsStringAsync();

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task MultiFactorLogin_Returns401Unauthorized_WhenLoginIsUnsuccessful()
    {
        var signInManagerMock = new SignInManagerMock<User>().Mock();

        signInManagerMock.Setup(x => x.GetTwoFactorAuthenticationUserAsync()).ReturnsAsync(new User() { Email = Seed.Test.MultiFactorUserEmail, TwoFactorEnabled = true} );
        signInManagerMock.Setup(x => x.TwoFactorSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(SignInResult.Failed);
        
        var client = _fixture.WebApplicationFactory.WithWebHostBuilder(c => 
        {
            c.ConfigureServices(s => 
            {
                s.Replace(new ServiceDescriptor(typeof(SignInManager<User>), signInManagerMock.Object));
            });
        }).CreateClient();

        var request = new MultiFactorLoginRequest()
        {
            Token = "888888" // must be 6 chars.
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        using var sut = await client.PostAsync($"/api/v1/{Routes.Users.MultiFactorLogin}", jsonContent);

        var errorResponse = await sut.Content.ReadAsStringAsync();

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }
}