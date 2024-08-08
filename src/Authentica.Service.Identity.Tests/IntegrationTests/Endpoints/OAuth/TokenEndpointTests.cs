using Api.Constants;
using Api.Responses;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

[TestFixture]
public class TokenEndpointTests
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
    public async Task TokenEndpoint_ReturnsOK_WhenClientCredentialsAreUsed()
    {
        var values = new Dictionary<string, string>
        {
            { "client_id", "2e5cf15b-bf5b-4d80-aa01-2a596403530d" },
            { "client_secret", "eCp79BsVS5uPb7J6MDStjfuw8h1Jv5dSKA89epAtsLy4pyGgJ6IjIfDeibTtXz7uGEMQixQl/XFjfwCUj7esNn0xUkwobzqHVJN43YLZcIZzyV5yLqKKE/Ku/YsVkZqg5/9eMi4jOKsuxGBRbMA9KeNeFk9TYybwXYbpoQTeHg8dvilNy0NsLzcZ9leD9IVmo5hhMmB9n9ghl1U/R6gCjwMaQY8alFntWSnu7SFJkNAv2o6pmaQTFwGQ7b+wl0lTKdASMQZoj/IVlEXwNNz2OOUCUnBTj5rza9ovs5KgyuwsURIBMe6w9DoEBsjtdoqco/o6nNABrmuB66yg==" },
            { "grant_type", "client_credentials" },
            { "redirect_uri", "https://localhost:7256"}
        };

        var content = new FormUrlEncodedContent(values);

        var result = await _fixture.Client.PostAsync($"api/v1/{Routes.OAuth.Token}", content);
        result.EnsureSuccessStatusCode();

        var jsonResponse = await result.Content.ReadFromJsonAsync(typeof(TokenResponse));

        var root = (TokenResponse)jsonResponse!;

        var token = root.AccessToken;

        Assert.Multiple(() =>
        {
            Assert.That(token, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        });

    }

    [Test]
    public async Task TokenEndpoint_ReturnsOK_WhenRefreshTokenIsUsed()
    {
        var values = new Dictionary<string, string>
        {
            { "client_id", "2e5cf15b-bf5b-4d80-aa01-2a596403530d" },
            { "client_secret", "eCp79BsVS5uPb7J6MDStjfuw8h1Jv5dSKA89epAtsLy4pyGgJ6IjIfDeibTtXz7uGEMQixQl/XFjfwCUj7esNn0xUkwobzqHVJN43YLZcIZzyV5yLqKKE/Ku/YsVkZqg5/9eMi4jOKsuxGBRbMA9KeNeFk9TYybwXYbpoQTeHg8dvilNy0NsLzcZ9leD9IVmo5hhMmB9n9ghl1U/R6gCjwMaQY8alFntWSnu7SFJkNAv2o6pmaQTFwGQ7b+wl0lTKdASMQZoj/IVlEXwNNz2OOUCUnBTj5rza9ovs5KgyuwsURIBMe6w9DoEBsjtdoqco/o6nNABrmuB66yg==" }, // Add your client secret here
            { "grant_type", "refresh_token" },
            { "refresh_token", "abddhdhdh"},
            { "redirect_uri", "https://localhost:7256"}
        };

        var content = new FormUrlEncodedContent(values);

        var mockJwtProvider = new JsonWebTokenProviderMock();

        mockJwtProvider.Setup(x => x.TryValidateTokenAsync(It.IsAny<string>(),
                                                           It.IsAny<string>(),
                                                           It.IsAny<string>(),
                                                           It.IsAny<string>())).ReturnsAsync(new JwtResult(){ Success = true});

        mockJwtProvider.Setup(x => x.TryCreateTokenAsync(It.IsAny<string>(),
                                                         It.IsAny<string>(),
                                                         It.IsAny<string>(),
                                                         It.IsAny<string>(),
                                                         It.IsAny<int>(),
                                                         It.IsAny<string>())).ReturnsAsync(new JwtResult() { Success = true, Token = "asjsjsjsj"});

        mockJwtProvider.Setup(x => x.TryCreateRefreshTokenAsync(It.IsAny<string>(),
                                                                It.IsAny<string>(),
                                                                It.IsAny<string>(),
                                                                It.IsAny<string>(),
                                                                It.IsAny<int>(),
                                                                It.IsAny<string>())).ReturnsAsync(new JwtResult() { Success = true, Token = "asjsjsjsj"});

        var sutClient = _fixture.WebApplicationFactory.WithWebHostBuilder(x => 
        {
            x.ConfigureTestServices(s => 
            {
                s.Replace(new ServiceDescriptor(typeof(IJsonWebTokenProvider), mockJwtProvider.Object));
            });
        }).CreateClient();

        var result = await sutClient.PostAsync($"api/v1/{Routes.OAuth.Token}", content);
        result.EnsureSuccessStatusCode();

         var jsonResponse = await result.Content.ReadFromJsonAsync(typeof(TokenResponse));

        var root = (TokenResponse)jsonResponse!;

        var token = root.AccessToken;

        var refreshToken = root.RefreshToken;

        Assert.Multiple(() =>
        {
            Assert.That(token, Is.Not.Null);
            Assert.That(refreshToken, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        });

    }

    [Test]
    public async Task TokenEndpoint_ReturnsOK_WhenCodeIsUsed()
    {
        var values = new Dictionary<string, string>
        {
            { "client_id", "2e5cf15b-bf5b-4d80-aa01-2a596403530d" },
            { "client_secret", "eCp79BsVS5uPb7J6MDStjfuw8h1Jv5dSKA89epAtsLy4pyGgJ6IjIfDeibTtXz7uGEMQixQl/XFjfwCUj7esNn0xUkwobzqHVJN43YLZcIZzyV5yLqKKE/Ku/YsVkZqg5/9eMi4jOKsuxGBRbMA9KeNeFk9TYybwXYbpoQTeHg8dvilNy0NsLzcZ9leD9IVmo5hhMmB9n9ghl1U/R6gCjwMaQY8alFntWSnu7SFJkNAv2o6pmaQTFwGQ7b+wl0lTKdASMQZoj/IVlEXwNNz2OOUCUnBTj5rza9ovs5KgyuwsURIBMe6w9DoEBsjtdoqco/o6nNABrmuB66yg==" },
            { "grant_type", "code" },
            { "code", null!},
            { "state", null!},
            { "redirect_uri", "https://localhost:7256"}
        };

        var content = new FormUrlEncodedContent(values);

        var mockJwtProvider = new JsonWebTokenProviderMock();
        var mockHttpContext = new HttpContextMock();
        var mockSession = new Mock<ISession>();

        mockHttpContext.Setup(x => x.Session).Returns(mockSession.Object);


        mockJwtProvider.Setup(x => x.TryValidateTokenAsync(It.IsAny<string>(),
                                                           It.IsAny<string>(),
                                                           It.IsAny<string>(),
                                                           It.IsAny<string>())).ReturnsAsync(new JwtResult(){ Success = true});

        mockJwtProvider.Setup(x => x.TryCreateTokenAsync(It.IsAny<string>(),
                                                         It.IsAny<string>(),
                                                         It.IsAny<string>(),
                                                         It.IsAny<string>(),
                                                         It.IsAny<int>(),
                                                         It.IsAny<string>())).ReturnsAsync(new JwtResult() { Success = true, Token = "asjsjsjsj"});

        mockJwtProvider.Setup(x => x.TryCreateRefreshTokenAsync(It.IsAny<string>(),
                                                                It.IsAny<string>(),
                                                                It.IsAny<string>(),
                                                                It.IsAny<string>(),
                                                                It.IsAny<int>(),
                                                                It.IsAny<string>())).ReturnsAsync(new JwtResult() { Success = true, Token = "asjsjsjsj"});

        var sutClient = _fixture.WebApplicationFactory.WithWebHostBuilder(x => 
        {
            x.ConfigureTestServices(s => 
            {
                s.Replace(new ServiceDescriptor(typeof(IJsonWebTokenProvider), mockJwtProvider.Object));
            });
        }).CreateClient();

        var result = await sutClient.PostAsync($"api/v1/{Routes.OAuth.Token}", content);
        result.EnsureSuccessStatusCode();

        var jsonResponse = await result.Content.ReadFromJsonAsync(typeof(TokenResponse));

        var root = (TokenResponse)jsonResponse!;

        var token = root.AccessToken;

        var refreshToken = root.RefreshToken;

        Assert.Multiple(() =>
        {
            Assert.That(token, Is.Not.Null);
            Assert.That(refreshToken, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        });

    }
}
