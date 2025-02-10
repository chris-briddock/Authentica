using Domain.Responses;

namespace Authentica.Service.Identity.Tests.IntegrationTests;

[TestFixture]
public class TestFixture<TProgram> where TProgram : class
{
    public CustomWebApplicationFactory<TProgram> WebApplicationFactory { get; private set; }

    public string AccessToken { get; private set; }

    public HttpClient Client { get; private set; }

    [OneTimeSetUp]
    public async Task OneTimeSetUpAsync()
    {
        WebApplicationFactory = new CustomWebApplicationFactory<TProgram>();
        WebApplicationFactory.StartTestContainer();
        Client = WebApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = true,
            HandleCookies = true
        });
        await GenerateTokenAsync(); 
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        Client?.Dispose();
        WebApplicationFactory.StopTestContainer();
        WebApplicationFactory.Dispose();
    }

    private async Task GenerateTokenAsync()
    {
        var values = new Dictionary<string, string>
        {
            { "client_id", Seed.Test.TestClientId },
            { "client_secret", Seed.Secret },
            { "grant_type", "client_credentials" }
        };

        var content = new FormUrlEncodedContent(values);

        var result = await Client.PostAsync($"api/v2/{Routes.OAuth.Token}", content);

        result.EnsureSuccessStatusCode();

        var jsonResponse = await result.Content.ReadFromJsonAsync(typeof(TokenResponse));

        var response = (TokenResponse)jsonResponse!;

        AccessToken = response.AccessToken;
    }

    public HttpClient CreateAuthenticatedClient(Action<IServiceCollection> configureServices = null!)
    {
        var client = WebApplicationFactory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                configureServices?.Invoke(services);
            });
        }).CreateClient(new WebApplicationFactoryClientOptions()
        {
            AllowAutoRedirect = true
        });

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
        return client;
    }
}
