using Application.BackgroundServices;
using Persistence.Contexts;

namespace Authentica.Service.Identity.Tests.IntegrationTests;

public class ApplicationPurgeExposeProtected : ApplicationPurge
{
    public ApplicationPurgeExposeProtected(IServiceScopeFactory serviceScopeFactory,
                                                    ILogger<ApplicationPurge> logger,
                                                    Application.Contracts.ITimer timer)
        : base(serviceScopeFactory, logger, timer)
    { }

    // Expose ExecuteAsync
    public Task ExecuteTaskAsync(CancellationToken cancellationToken)
    {
        return base.ExecuteAsync(cancellationToken);
    }
}

[TestFixture]
public class ApplicationPurgeBackgroundServiceTests
{
    private TestFixture<Program> _fixture;

    public ApplicationPurgeBackgroundServiceTests()
    {
        _fixture = new TestFixture<Program>();
        _fixture.OneTimeSetUp();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _fixture.OneTimeTearDown();
        _fixture.Dispose();
        _fixture = null!;
    }

    [Test]
    public async Task ExecuteAsync_DeletesOldUserApplicationsAfterSevenYears()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<ApplicationPurge>>();
        var mockTimer = new TimerMock();

        var timerCallCount = 0;
        mockTimer.Setup(t => t.WaitForNextTickAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(() =>
            {
                timerCallCount++;
                return timerCallCount == 1;
            });

        var webAppFactory = _fixture.WebApplicationFactory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.Replace(ServiceDescriptor.Singleton(mockTimer.Object));
            });
        });

        var scopeFactory = _fixture.WebApplicationFactory.Services.GetService<IServiceScopeFactory>()!;


        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var service = new ApplicationPurgeExposeProtected(scopeFactory, mockLogger.Object, mockTimer.Object);

        var oldDeletedApp = dbContext.ClientApplications
        .Where(x => x.DeletedOnUtc <= DateTime.UtcNow.AddYears(-8))
        .FirstOrDefault()!;

        var recentDeletedApp = dbContext.ClientApplications
                                .Where(x => x.Name == "Default Recent Deleted Application")
                                .FirstOrDefault()!;

        // Act
        await service.ExecuteTaskAsync(CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(dbContext.ClientApplications.FirstOrDefault(u => u == oldDeletedApp), Is.Null);
            Assert.That(dbContext.ClientApplications.Any(u => u.Id == recentDeletedApp.Id), Is.True);
        });
    }

}