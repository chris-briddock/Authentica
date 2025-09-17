namespace Authentica.Service.Identity.Tests.IntegrationTests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
                                                     .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                                                     .WithPortBinding(1433, true)
                                                     .WithWaitStrategy(Wait.ForUnixContainer().UntilMessageIsLogged("SQL Server is now ready for client connections"))
                                                     .WithAutoRemove(true)
                                                     .Build();
    private readonly RedisContainer _redisContainer = new RedisBuilder()
                                                    .WithImage("redis:latest")
                                                    .WithPortBinding(6379, true)
                                                    .WithWaitStrategy(Wait.ForUnixContainer().UntilMessageIsLogged("Ready to accept connections tcp"))
                                                    .WithAutoRemove(true)
                                                    .Build();

    public void StartTestContainer()
    {
        _msSqlContainer.StartAsync().Wait();
        _redisContainer.StartAsync().Wait();
    }
    public void StopTestContainer()
    {
        _msSqlContainer.StopAsync().Wait();
        _msSqlContainer.DisposeAsync().AsTask().Wait();

        _redisContainer.StopAsync().Wait();
        _redisContainer.DisposeAsync().AsTask().Wait();
    }
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var connectionString = _msSqlContainer.GetConnectionString().Replace("master", "Authentica.Service.Identity", StringComparison.Ordinal);
        string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        builder.UseEnvironment(env);

        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
            logging.SetMinimumLevel(LogLevel.Trace);
        });

        builder.ConfigureAppConfiguration((context, config) =>
        {
            var redisConnectionString = $"{_redisContainer.Hostname}:{_redisContainer.GetMappedPublicPort(6379)},ssl=False,abortConnect=False";

            config.AddInMemoryCollection(
            [
                new KeyValuePair<string, string?>("ConnectionStrings:DefaultConnection", connectionString),
                new KeyValuePair<string, string?>("ConnectionStrings:Redis", redisConnectionString),
                new KeyValuePair<string, string?>("Defaults:AdminEmail", "admin2@default.com"),
                new KeyValuePair<string, string?>("Defaults:AdminPassword", "fRpGWqvn4Mu,6w[Z8axP;b5="),
                new KeyValuePair<string, string?>("Defaults:Secret", "b0ahmqtOMNVTnTUJ4E19QNSFe8UYOHqDoO9ovXbNSnRrHjrMbYc1gREBqFOL8XZXuEDFhGamf4Teq7HfXqjMm4kLqjGCg7XAqCjDdUaPSm2HCS2hEL8wR2zD"),
                new KeyValuePair<string, string?>("Defaults:CallbackUri", "https://localhost:7256/callback"),
                new KeyValuePair<string, string?>("Jwt:Issuer", "https://localhost:7171"),
                new KeyValuePair<string, string?>("Jwt:Audience", "https://localhost:7171"),
                new KeyValuePair<string, string?>("Jwt:Secret", "y&>tq_:|8@$u81vM(#kQ;{]|3Adx>m!sSrpkS]iy^Nn|'zjG;s;FDhDLjJEF{/H',FJ~[aJ~qg0$q@!iIeV"),
                new KeyValuePair<string, string?>("Jwt:Expires", "3600"),
                new KeyValuePair<string, string?>("FeatureManagement:Cache", "true"),
                new KeyValuePair<string, string?>("FeatureManagement:AppInsights", "false"),
                new KeyValuePair<string, string?>("FeatureManagement:ServiceBus", "false"),
                new KeyValuePair<string, string?>("FeatureManagement:RabbitMq", "false"),

            ]).Build();
        });
    }
}
