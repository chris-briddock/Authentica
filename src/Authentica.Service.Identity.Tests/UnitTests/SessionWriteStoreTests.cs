using Application.Stores;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Persistence.Contexts;

namespace Authentica.Service.Identity.Tests.UnitTests;

public class SessionWriteStoreTests
{
    private ServiceProviderMock _serviceProviderMock;
    private DbContextMock _dbContextMock;
    private DbSetMock<Session> _dbSetMock;
    private SessionWriteStore _sut;

    [SetUp]
    public void SetUp()
    {
        // Initialize the mocks using the mock class templates
        _dbContextMock = new DbContextMock();
        _dbSetMock = new DbSetMock<Session>();
        _serviceProviderMock = new ServiceProviderMock();

        // Set up the DbContext to return the mocked DbSet
        _dbContextMock.Setup(x => x.Set<Session>()).Returns(_dbSetMock.Object);

        // Set up the IServiceProvider to return the mocked DbContext
        _serviceProviderMock.Setup(x => x.GetService(typeof(AppDbContext))).Returns(_dbContextMock.Object);

        // Initialize the SessionWriteStore with the mocked IServiceProvider
        _sut = new SessionWriteStore(_serviceProviderMock.Object);
    }

    [Test]
    public async Task CreateAsync_Should_AddSessionToDbSet_And_SaveChanges()
    {
        // Arrange
        var session = new Session { Id = Guid.NewGuid().ToString() };

        // Mock the behavior of AddAsync and SaveChangesAsync
        _dbSetMock.Setup(x => x.AddAsync(session, default)).ReturnsAsync((EntityEntry<Session>)null!);
        _dbContextMock.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);

        // Act
        var result = await _sut.CreateAsync(session);

        // Assert
        Assert.That(session, Is.EqualTo(result));
        _dbSetMock.Verify(x => x.AddAsync(session, default), Times.Once);
        _dbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    [Test]
    public void CreateAsync_Should_ThrowException_When_AddAsyncFails()
    {
        // Arrange
        var session = new Session
        {
            Id = Guid.NewGuid().ToString(),
            SessionId = "test-session-id",
            UserId = "test-user-id",
            StartDateTime = DateTime.UtcNow,
            Status = "Active",
            UserAgent = "Mozilla/5.0",
        };

        // Simulate an exception being thrown when AddAsync is called
        _dbSetMock.Setup(x => x.AddAsync(session, default)).ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () => await _sut.CreateAsync(session));
        Assert.That(ex.Message, Is.EqualTo("Database error"));

        // Verify that SaveChangesAsync was never called since AddAsync failed
        _dbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Never);
    }
}