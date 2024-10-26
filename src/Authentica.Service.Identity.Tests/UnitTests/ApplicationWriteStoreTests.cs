using Application.Contracts;
using Application.DTOs;
using Application.Factories;
using Application.Stores;
using Domain.ValueObjects;
using Persistence.Contexts;

namespace Authentica.Service.Identity.Tests.UnitTests;

public class ApplicationWriteStoreTests
{
    private UserReadStoreMock _userReadStoreMock;
    private DbContextMock _dbContextMock;
    private ApplicationReadStoreMock _applicationReadStoreMock;
    private ApplicationWriteStore _sut;

    [SetUp]
    public void SetUp()
    {
        _userReadStoreMock = new UserReadStoreMock();
        _dbContextMock = new DbContextMock();
        _applicationReadStoreMock = new ApplicationReadStoreMock();
        var services = new ServiceProviderMock();
        services.Setup(x => x.GetService(typeof(IUserReadStore))).Returns(_userReadStoreMock.Object);
        services.Setup(x => x.GetService(typeof(AppDbContext))).Returns(_dbContextMock.Object);

        services.Setup(x => x.GetService(typeof(IUserReadStore))).Returns(_userReadStoreMock.Object);
        services.Setup(x => x.GetService(typeof(IApplicationReadStore))).Returns(_applicationReadStoreMock.Object);
        services.Setup(x => x.GetService(typeof(AppDbContext))).Returns(_dbContextMock.Object);

        _sut = new ApplicationWriteStore(services.Object);
    }

    // Test case: when an exception is thrown
    [Test]
    public async Task CreateClientApplicationAsync_Should_ReturnFailedResult_WhenExceptionThrown()
    {
        // Arrange
        var dto = new ApplicationDto<CreateApplicationRequest>
        {
            ClaimsPrincipal = new ClaimsPrincipal(),
            Request = new CreateApplicationRequest
            {
                Name = "Test App",
                CallbackUri = "http://callback.com"
            }
        };

        // Simulate an exception being thrown during GetUserByEmailAsync
        _userReadStoreMock.Setup(x => x.GetUserByEmailAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _sut.CreateClientApplicationAsync(dto, CancellationToken.None);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.Succeeded, Is.False);
            Assert.That(result.Errors.First().Code, Is.EqualTo(IdentityErrorFactory.ExceptionOccurred(new Exception("Test exception")).Code));
        });
    }

    [Test]
    public async Task UpdateApplicationAsync_Should_ReturnFailedResult_When_UserIsNull()
    {
        // Arrange
        var dto = new ApplicationDto<UpdateApplicationByNameRequest>
        {
            ClaimsPrincipal = new ClaimsPrincipal(),
            Request = new UpdateApplicationByNameRequest
            {
                CurrentName = "OldAppName",
                NewName = "NewAppName"
            }
        };

        _userReadStoreMock.Setup(x => x.GetUserByEmailAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(UserStoreResult.Failed());

        // Act
        var result = await _sut.UpdateApplicationAsync(dto);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Succeeded, Is.False);
    }

    [Test]
    public async Task UpdateApplicationAsync_Should_ReturnFailedResult_When_ApplicationIsNull()
    {
        // Arrange
        var dto = new ApplicationDto<UpdateApplicationByNameRequest>
        {
            ClaimsPrincipal = new ClaimsPrincipal(),
            Request = new UpdateApplicationByNameRequest
            {
                CurrentName = "OldAppName",
                NewName = "NewAppName"
            }
        };

        var user = new User { Id = "test-user-id" };
        _userReadStoreMock.Setup(x => x.GetUserByEmailAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(UserStoreResult.Failed());

        _applicationReadStoreMock.Setup(x => x.GetClientApplicationByNameAndUserIdAsync(
            dto.Request.CurrentName, user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ClientApplication)null!);

        // Act
        var result = await _sut.UpdateApplicationAsync(dto);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Succeeded, Is.False);
    }

    [Test]
    public async Task UpdateApplicationAsync_Should_ReturnFailedResult_When_ExceptionIsThrown()
    {
        // Arrange
        var dto = new ApplicationDto<UpdateApplicationByNameRequest>
        {
            ClaimsPrincipal = new ClaimsPrincipal(),
            Request = new UpdateApplicationByNameRequest
            {
                CurrentName = "OldAppName",
                NewName = "NewAppName"
            }
        };

        var user = new User { Id = "test-user-id" };
        _userReadStoreMock.Setup(x => x.GetUserByEmailAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(UserStoreResult.Success(user));

        _applicationReadStoreMock.Setup(x => x.GetClientApplicationByNameAndUserIdAsync(
            dto.Request.CurrentName, user.Id, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _sut.UpdateApplicationAsync(dto);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Succeeded, Is.False);
    }

    [Test]
    public async Task SoftDeleteApplicationAsync_Should_ReturnFailedResult_When_UserIsNull()
    {
        // Arrange
        var dto = new ApplicationDto<DeleteApplicationByNameRequest>
        {
            ClaimsPrincipal = new ClaimsPrincipal(),
            Request = new DeleteApplicationByNameRequest
            {
                Name = "AppName"
            }
        };

        _userReadStoreMock.Setup(x => x.GetUserByEmailAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(UserStoreResult.Failed());

        // Act
        var result = await _sut.SoftDeleteApplicationAsync(dto);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Succeeded, Is.False);
    }

    [Test]
    public async Task SoftDeleteApplicationAsync_Should_ReturnFailedResult_When_ApplicationIsNull()
    {
        // Arrange
        var dto = new ApplicationDto<DeleteApplicationByNameRequest>
        {
            ClaimsPrincipal = new ClaimsPrincipal(),
            Request = new DeleteApplicationByNameRequest
            {
                Name = "AppName"
            }
        };

        var user = new User { Id = "test-user-id" };
        _userReadStoreMock.Setup(x => x.GetUserByEmailAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(UserStoreResult.Success(user));

        _applicationReadStoreMock.Setup(x => x.GetClientApplicationByNameAndUserIdAsync(
            dto.Request.Name, user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ClientApplication)null!);

        // Act
        var result = await _sut.SoftDeleteApplicationAsync(dto);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Succeeded, Is.False);
    }

    [Test]
    public async Task SoftDeleteApplicationAsync_Should_ReturnFailedResult_When_ExceptionIsThrown()
    {
        // Arrange
        var dto = new ApplicationDto<DeleteApplicationByNameRequest>
        {
            ClaimsPrincipal = new ClaimsPrincipal(),
            Request = new DeleteApplicationByNameRequest
            {
                Name = "AppName"
            }
        };

        var user = new User { Id = "test-user-id" };
        _userReadStoreMock.Setup(x => x.GetUserByEmailAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(UserStoreResult.Success(user));

        _applicationReadStoreMock.Setup(x => x.GetClientApplicationByNameAndUserIdAsync(
            dto.Request.Name, user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ClientApplication());

        _dbContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                      .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _sut.SoftDeleteApplicationAsync(dto);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Succeeded, Is.False);
    }

    [Test]
    public async Task SoftDeleteApplicationAsync_Should_ReturnSuccess_When_ApplicationIsDeleted()
    {
        // Arrange
        var dto = new ApplicationDto<DeleteApplicationByNameRequest>
        {
            ClaimsPrincipal = new ClaimsPrincipal(),
            Request = new DeleteApplicationByNameRequest
            {
                Name = "AppName"
            }
        };

        var user = new User { Id = "test-user-id" };
        var application = new ClientApplication
        {
            EntityDeletionStatus = new EntityDeletionStatus<string>(false, null, null),
            EntityCreationStatus = new EntityCreationStatus<string>(DateTime.UtcNow, null!),
            EntityModificationStatus = new EntityModificationStatus<string>(DateTime.UtcNow, null)
        };

        // Create a mock for DbSet<ClientApplication>
        var applicationDbSetMock = new DbSetMock<ClientApplication>();
        applicationDbSetMock.Setup(m => m.Update(It.IsAny<ClientApplication>())).Callback<ClientApplication>(app =>
        {
            application.EntityDeletionStatus = app.EntityDeletionStatus; // Simulate updating the entity
        });

        // Set up the mock for the DbContext
        _dbContextMock.Setup(m => m.Set<ClientApplication>()).Returns(applicationDbSetMock.Object);
        _dbContextMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Set up user read store mock
        _userReadStoreMock.Setup(x => x.GetUserByEmailAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(UserStoreResult.Success(user));

        // Set up application read store mock
        _applicationReadStoreMock.Setup(x => x.GetClientApplicationByNameAndUserIdAsync(
            dto.Request.Name, user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(application);

        // Act
        var result = await _sut.SoftDeleteApplicationAsync(dto);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Succeeded, Is.True);
            Assert.That(application.EntityDeletionStatus.IsDeleted, Is.True);
            Assert.That(application.EntityDeletionStatus.DeletedBy, Is.EqualTo(user.Id));
            Assert.That(application.EntityDeletionStatus.DeletedOnUtc, Is.Not.Null);
        });
    }
}