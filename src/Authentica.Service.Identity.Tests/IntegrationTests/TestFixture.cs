using Api.Constants;
using Api.Responses;

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
            { "client_id", "2e5cf15b-bf5b-4d80-aa01-2a596403530d" },
            { "client_secret", "eCp79BsVS5uPb7J6MDStjfuw8h1Jv5dSKA89epAtsLy4pyGgJ6IjIfDeibTtXz7uGEMQixQl/XFjfwCUj7esNn0xUkwobzqHVJN43YLZcIZzyV5yLqKKE/Ku/YsVkZqg5/9eMi4jOKsuxGBRbMA9KeNeFk9TYybwXYbpoQTeHg8dvilNy0NsLzcZ9leD9IVmo5hhMmB9n9ghl1U/R6gCjwMaQY8alFntWSnu7SFJkNAv2o6pmaQTFwGQ7b+wl0lTKdASMQZoj/IVlEXwNNz2OOUCUnBTj5rza9ovs5KgyuwsURIBMe6w9DoEBsjtdoqco/o6nNABrmuB66yg==" }, // Add your client secret here
            { "grant_type", "client_credentials" }
        };

        var content = new FormUrlEncodedContent(values);

        var result = await Client.PostAsync($"api/v1/{Routes.OAuth.Token}", content);

        var errorContent = await result.Content.ReadAsStringAsync();

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
