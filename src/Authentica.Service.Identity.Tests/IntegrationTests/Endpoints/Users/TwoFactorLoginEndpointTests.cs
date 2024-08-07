using System.Text;
using Api.Constants;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

public class TwoFactorLoginEndpointTests
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
    public async Task TwoFactorLogin_Returns401Unauthorized_WhenTwoFactorIsNotEnabledSuccessful()
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

        var request = new TwoFactorLoginRequest()
        {
            EmailAddress = "twofactortest@default.com",
            Token = "888888" // must be 6 chars.
        };

        var jsonContent = new StringContent(JsonSerializer.ToJsonString(request), Encoding.UTF8, "application/json");

        using var sut = await client.PostAsync($"/api/v1/{Routes.Users.TwoFactorLogin}", jsonContent);

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task TwoFactorLogin_Returns400BadRequest_WhenUserIsNotFound()
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

        var request = new TwoFactorLoginRequest()
        {
            EmailAddress = "twofactortest@default.com",
            Token = "888888" // must be 6 chars.
        };

        var jsonContent = new StringContent(JsonSerializer.ToJsonString(request), Encoding.UTF8, "application/json");

        using var sut = await client.PostAsync($"/api/v1/{Routes.Users.TwoFactorLogin}", jsonContent);

        

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task TwoFactorLogin_Returns200OK_WhenLoginIsSuccessful()
    {
        var signInManagerMock = new SignInManagerMock<User>().Mock();

        signInManagerMock.Setup(x => x.GetTwoFactorAuthenticationUserAsync()).ReturnsAsync(new User() { Email = "twoFactorTest@default.com", TwoFactorEnabled = true} );
        signInManagerMock.Setup(x => x.TwoFactorSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(SignInResult.Success);
        
        var client = _fixture.WebApplicationFactory.WithWebHostBuilder(c => 
        {
            c.ConfigureServices(s => 
            {
                s.Replace(new ServiceDescriptor(typeof(SignInManager<User>), signInManagerMock.Object));
            });
        }).CreateClient();

        var request = new TwoFactorLoginRequest()
        {
            EmailAddress = "twoFactorTest@default.com",
            Token = "888888" // must be 6 chars.
        };

        var jsonContent = new StringContent(JsonSerializer.ToJsonString(request), Encoding.UTF8, "application/json");

        using var sut = await client.PostAsync($"/api/v1/{Routes.Users.TwoFactorLogin}", jsonContent);

        var errorResponse = await sut.Content.ReadAsStringAsync();

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task TwoFactorLogin_Returns401Unauthorized_WhenLoginIsUnsuccessful()
    {
        var signInManagerMock = new SignInManagerMock<User>().Mock();

        signInManagerMock.Setup(x => x.GetTwoFactorAuthenticationUserAsync()).ReturnsAsync(new User() { Email = "twoFactorTest@default.com", TwoFactorEnabled = true} );
        signInManagerMock.Setup(x => x.TwoFactorSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(SignInResult.Failed);
        
        var client = _fixture.WebApplicationFactory.WithWebHostBuilder(c => 
        {
            c.ConfigureServices(s => 
            {
                s.Replace(new ServiceDescriptor(typeof(SignInManager<User>), signInManagerMock.Object));
            });
        }).CreateClient();

        var request = new TwoFactorLoginRequest()
        {
            EmailAddress = "twoFactorTest@default.com",
            Token = "888888" // must be 6 chars.
        };

        var jsonContent = new StringContent(JsonSerializer.ToJsonString(request), Encoding.UTF8, "application/json");

        using var sut = await client.PostAsync($"/api/v1/{Routes.Users.TwoFactorLogin}", jsonContent);

        var errorResponse = await sut.Content.ReadAsStringAsync();

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }
}