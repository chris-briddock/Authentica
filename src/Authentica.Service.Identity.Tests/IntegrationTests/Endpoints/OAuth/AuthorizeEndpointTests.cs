using System.Text;
using System.Web;
using Api.Constants;
using Microsoft.Extensions.Primitives;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

public class AuthorizeEndpointTests
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
    public async Task AuthorizeEndpoint_Returns301Moved_WhenCodeIsRequestedForRegisteredApplication()
    {
        using var client = _fixture.WebApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        var values = new Dictionary<string, StringValues>()
        {
            { "client_id", new StringValues("2e5cf15b-bf5b-4d80-aa01-2a596403530d")},
            { "callback_uri", new StringValues("https://localhost:7256/callback")},
            { "response_type", new StringValues("code")}

        };

        var queryValues = new QueryCollection(values);

        // Build the query string
        var queryString = new StringBuilder();
        foreach (var key in queryValues.Keys)
        {
            foreach (var value in queryValues[key])
            {
                if (queryString.Length > 0)
                {
                    queryString.Append('&');
                }
                queryString.Append($"{HttpUtility.UrlEncode(key)}={HttpUtility.UrlEncode(value)}");
            }
        }

        using var response = await client.GetAsync($"api/v1/{Routes.OAuth.Authorize}?{queryString}");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.MovedPermanently));

    }
}
