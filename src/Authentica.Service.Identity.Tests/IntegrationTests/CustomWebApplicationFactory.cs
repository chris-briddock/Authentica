using System.Drawing;

namespace Authentica.Service.Identity.Tests.IntegrationTests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private MsSqlContainer _msSqlContainer = new MsSqlBuilder()
                                                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                                                .WithWaitStrategy(Wait.ForUnixContainer())
                                                .WithAutoRemove(true)
                                                .Build();

    private IContainer _redisCacheContainer = new ContainerBuilder()
                                             .WithImage("redis:latest")
                                             .WithWaitStrategy(Wait.ForUnixContainer())
                                             .WithPortBinding(5002, true)
                                             .WithAutoRemove(true)
                                             .Build();
    private IContainer _loggingContainer = new ContainerBuilder()
                                             .WithImage("mcr.microsoft.com/dotnet/aspire-dashboard:8.0.0")
                                             .WithWaitStrategy(Wait.ForUnixContainer())
                                             .WithPortBinding(4317, true)
                                             .WithAutoRemove(true)
                                             .Build();

    public IContainer _messagingContainer = new ContainerBuilder()
                                            .WithImage("rabbitmq:latest")
                                            .WithWaitStrategy(Wait.ForUnixContainer())
                                            .WithPortBinding(5672, false)
                                            .Build();

    public IContainer _testClientContainer = new ContainerBuilder()
                                            .WithImage("immerslve/testclient")
                                            .WithWaitStrategy(Wait.ForUnixContainer())
                                            .WithPortBinding(7256)
                                            .Build();

    public void StartTestContainer()
    {
        _msSqlContainer.StartAsync().Wait();
        _redisCacheContainer.StartAsync().Wait();
        _loggingContainer.StartAsync().Wait();
        _messagingContainer.StartAsync().Wait();
        Task.Delay(TimeSpan.FromSeconds(30)).Wait();
    }
    public void StopTestContainer()
    {
        _msSqlContainer.StopAsync().Wait();
        _msSqlContainer.DisposeAsync().AsTask().Wait();
        _loggingContainer.StopAsync().Wait();
        _loggingContainer.DisposeAsync().AsTask().Wait();
        _redisCacheContainer.StopAsync().Wait();
        _redisCacheContainer.DisposeAsync().AsTask().Wait();
        _messagingContainer.StopAsync().Wait();
        _messagingContainer.DisposeAsync().AsTask().Wait();
    }
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var connectionString = _msSqlContainer.GetConnectionString();

        builder.UseEnvironment("Development");

        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddInMemoryCollection(
            [
                new KeyValuePair<string, string?>("ConnectionStrings:Default", _msSqlContainer.GetConnectionString()),
                new KeyValuePair<string, string?>("ConnectionStrings:Redis", $"localhost:{_redisCacheContainer.GetMappedPublicPort(5002)}"),
            ]).Build();
        });
    }

    protected override void ConfigureClient(HttpClient client)
    {
        base.ConfigureClient(client);
    }
}
