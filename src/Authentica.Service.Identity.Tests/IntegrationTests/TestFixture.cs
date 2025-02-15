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
        Client.Dispose();
        WebApplicationFactory.StopTestContainer();
        WebApplicationFactory.Dispose();
    }

    private async Task GenerateTokenAsync()
    {
        var values = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", "2e5cf15b-bf5b-4d80-aa01-2a596403530d" },
            { "client_secret", Seed.Secret },
            { "scopes", "read write"}
        };

        var content = new FormUrlEncodedContent(values);

        HttpResponseMessage? result = await Client.PostAsync($"api/v2/{Routes.OAuth.Token}", content);

        string? errorContent = await result.Content.ReadAsStringAsync();

        result.EnsureSuccessStatusCode();

        object? jsonResponse = await result.Content.ReadFromJsonAsync(typeof(TokenResponse));

        TokenResponse response = (TokenResponse)jsonResponse!;

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
