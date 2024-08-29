using Application.BackgroundServices;
using Application.Contracts;
using Application.Exceptions;
using Application.Factories;
using Persistence.Contexts;
using ITimer = Application.Contracts.ITimer;

namespace Authentica.Service.Identity.Tests.IntegrationTests;

public class ApplicationPurgeExposeProtected : ApplicationPurge
{
    public ApplicationPurgeExposeProtected(IServiceScopeFactory serviceScopeFactory,
                                           ILogger<ApplicationPurge> logger,
                                           ITimer timer)
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
                services.Replace(new ServiceDescriptor(typeof(ITimer), mockTimer.Object));
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

    [Test]
    public void ExecuteAsync_ThrowsException_WhenApplicationPurgeFails()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<ApplicationPurge>>();
        var mockTimer = new TimerMock();
        var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
        var serviceScopeMock = new Mock<IServiceScope>();
        var serviceProviderMock = new Mock<IServiceProvider>();
        var sharedStoreMock = new Mock<ISharedStore>();

        var timerCallCount = 0;
        mockTimer.Setup(t => t.WaitForNextTickAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(() =>
            {
                timerCallCount++;
                return timerCallCount == 1;
            });
        sharedStoreMock.Setup(x => x.PurgeEntriesAsync<ClientApplication>(It.IsAny<CancellationToken>()))
        .ReturnsAsync(SharedStoreResult.Failed(IdentityErrorFactory.ExceptionOccurred(new Exception())));
        
        serviceScopeMock.Setup(x => x.ServiceProvider.GetService(typeof(ISharedStore))).Returns(sharedStoreMock.Object);
        
        serviceScopeFactoryMock.Setup(x => x.CreateScope()).Returns(serviceScopeMock.Object);
        
        var webAppFactory = _fixture.WebApplicationFactory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.Replace(new ServiceDescriptor(typeof(ITimer), mockTimer.Object));
            });
        });

        var service = new ApplicationPurgeExposeProtected(serviceScopeFactoryMock.Object, mockLogger.Object, mockTimer.Object);

        Assert.ThrowsAsync<PurgeFailureException>(async () => await service.ExecuteTaskAsync(CancellationToken.None));
    }

}