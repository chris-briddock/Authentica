using Application.BackgroundServices;
using Application.Contracts;
using Application.Exceptions;
using Application.Factories;
using Persistence.Contexts;
using ITimer = Application.Contracts.ITimer;

namespace Authentica.Service.Identity.Tests.IntegrationTests;

public class AccountPurgeExposeProtected : AccountPurge
{
    public AccountPurgeExposeProtected(IServiceScopeFactory serviceScopeFactory,
                                       ILogger<AccountPurge> logger,
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
public class AccountPurgeBackgroundServiceTests
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
    public async Task ExecuteAsync_DeletesOldUserAccountsAfterSevenYears()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<AccountPurge>>();
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

        var service = new AccountPurgeExposeProtected(scopeFactory, mockLogger.Object, mockTimer.Object);

        var oldDeletedUser = dbContext.Users
        .Where(s => s.Email == "deletedUser@default.com")
        .Where(x => x.EntityDeletionStatus.DeletedOnUtc <= DateTime.UtcNow.AddYears(-8))
        .FirstOrDefault()!;

        var recentDeletedUser = dbContext.Users.Where(s => s.Email == "recentlydeleted@default.com").First();

        // Act
        await service.ExecuteTaskAsync(CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(dbContext.Users.FirstOrDefault(u => u == oldDeletedUser), Is.Null);
            Assert.That(dbContext.Users.Any(u => u.Id == recentDeletedUser.Id), Is.True);
        });
    }

    [Test]
    public void ExecuteAsync_ThrowsException_WhenAccountPurgeFails()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<AccountPurge>>();
        var mockTimer = new TimerMock();
        var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
        var serviceScopeMock = new Mock<IServiceScope>();
        var serviceProviderMock = new Mock<IServiceProvider>();
        var sharedStoreMock = new Mock<ISharedStore>();

         sharedStoreMock.Setup(x => x.PurgeEntriesAsync<User>(It.IsAny<CancellationToken>()))
        .ReturnsAsync(SharedStoreResult.Failed(IdentityErrorFactory.ExceptionOccurred(new Exception())));
        serviceScopeMock.Setup(x => x.ServiceProvider.GetService(typeof(ISharedStore))).Returns(sharedStoreMock.Object);
        serviceScopeFactoryMock.Setup(x => x.CreateScope()).Returns(serviceScopeMock.Object);

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

        var service = new AccountPurgeExposeProtected(serviceScopeFactoryMock.Object, mockLogger.Object, mockTimer.Object);
       
        Assert.ThrowsAsync<PurgeFailureException>(async () => await service.ExecuteTaskAsync(CancellationToken.None));
    }

}