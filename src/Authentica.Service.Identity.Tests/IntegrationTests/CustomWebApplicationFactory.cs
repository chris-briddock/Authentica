namespace Authentica.Service.Identity.Tests.IntegrationTests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
                                                     .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                                                     .WithWaitStrategy(Wait.ForUnixContainer().UntilMessageIsLogged("SQL Server is now ready for client connections"))
                                                     .WithAutoRemove(true)
                                                     .Build();

    public void StartTestContainer()
    {
        _msSqlContainer.StartAsync().Wait();
    }
    public void StopTestContainer()
    {
        _msSqlContainer.StopAsync().Wait();
        _msSqlContainer.DisposeAsync().AsTask().Wait();
    }
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var connectionString = _msSqlContainer.GetConnectionString().Replace("master", "Authentica.Service.Identity");
        string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        builder.UseEnvironment(env);

        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddInMemoryCollection(
            [
                new KeyValuePair<string, string?>("ConnectionStrings:DefaultConnection", connectionString),
                new KeyValuePair<string, string?>("Defaults:AdminEmail", "admin2@default.com"),
                new KeyValuePair<string, string?>("Defaults:AdminPassword", "fRpGWqvn4Mu,6w[Z8axP;b5="),
                new KeyValuePair<string, string?>("Defaults:Secret", "b0ahmqtOMNVTnTUJ4E19QNSFe8UYOHqDoO9ovXbNSnRrHjrMbYc1gREBqFOL8XZXuEDFhGamf4Teq7HfXqjMm4kLqjGCg7XAqCjDdUaPSm2HCS2hEL8wR2zD"),
                new KeyValuePair<string, string?>("Defaults:CallbackUri", "https://localhost:7256/callback"),
            ]).Build();
        });
    }
    protected override void ConfigureClient(HttpClient client)
    {
        base.ConfigureClient(client);
    }
}
