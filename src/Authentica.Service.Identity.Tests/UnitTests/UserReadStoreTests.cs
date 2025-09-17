using ZiggyCreatures.Caching.Fusion;

namespace Authentica.Service.Identity.Tests.UnitTests;

public class UserReadStoreTests
{
    private Mock<UserManager<User>> _userManagerMock;
    private ServiceProviderMock _serviceProviderMock;
    private FusionCacheMock _cacheMock;
    private UserReadStore _sut;
    private User _applicationUser;

    [SetUp]
    public void SetUp()
    {
        // Initialize the mock UserManager
        _userManagerMock = new UserManagerMock<User>().Mock();
        _cacheMock = new();

        _serviceProviderMock = new();

        // Setup IServiceProvider to return the mocked UserManager
        _serviceProviderMock.Setup(x => x.GetService(typeof(UserManager<User>)))
            .Returns(_userManagerMock.Object);
        _serviceProviderMock.Setup(x => x.GetService(typeof(IFusionCache)))
            .Returns(_cacheMock.Object);

        // Sample user for test cases
        _applicationUser = new User
        {
            Id = "user-id",
            Email = "user@example.com"
        };

        // Injecting the mocked service provider into UserReadStore
        _sut = new UserReadStore(_serviceProviderMock.Object);

    }

    [Test]
    public async Task GetUserByEmailAsync_WhenUserExists_ReturnsSuccessResult()
    {
        // Arrange
        var email = _applicationUser.Email!;

        _userManagerMock.Setup(x => x.FindByEmailAsync(email))
            .ReturnsAsync(_applicationUser);

        _cacheMock.Setup(x => x.GetOrSetAsync(
                It.IsAny<string>(),
                It.IsAny<Func<FusionCacheFactoryExecutionContext<User>, CancellationToken, Task<User>>>(),
                It.IsAny<MaybeValue<User>>(),
                It.IsAny<FusionCacheEntryOptions>(),
                It.IsAny<IEnumerable<string>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(_applicationUser);

        // Act
        var result = await _sut.GetUserByEmailAsync(email);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Succeeded, Is.True);
            Assert.That(result.User, Is.EqualTo(_applicationUser));
        });
    }

    [Test]
    public async Task GetUserByEmailAsync_WhenUserDoesNotExist_ReturnsFailedResult()
    {
        // Arrange
        var emailClaim = new Claim(ClaimTypes.Email, _applicationUser.Email!);
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity([emailClaim]));

        _userManagerMock.Setup(x => x.FindByEmailAsync(_applicationUser.Email!))
            .ReturnsAsync((User)null!); // Simulate user not found

        _cacheMock.Setup(x => x.GetOrSetAsync(
                It.IsAny<string>(),
                It.IsAny<Func<FusionCacheFactoryExecutionContext<User>, CancellationToken, Task<User>>>(),
                It.IsAny<MaybeValue<User>>(),
                It.IsAny<FusionCacheEntryOptions>(),
                It.IsAny<IEnumerable<string>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((User)null!);
        // Act
        var result = await _sut.GetUserByEmailAsync(claimsPrincipal);

        // Assert
        Assert.That(result.Succeeded, Is.False);
        Assert.That(result.Errors, Is.Not.Empty);
    }

    [Test]
    public async Task GetUserByEmailAsync_String_WhenUserDoesNotExist_ReturnsFailedResult()
    {
        // Arrange
        _userManagerMock.Setup(x => x.FindByEmailAsync(_applicationUser.Email!))
            .ReturnsAsync((User)null!); // Simulate user not found

        // Act
        var result = await _sut.GetUserByEmailAsync(_applicationUser.Email!);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.Succeeded, Is.False);
            Assert.That(result.Errors, Is.Not.Empty);
        });
    }

    [Test]
    public async Task GetUserByIdAsync_WhenUserExists_ReturnsSuccessResult()
    {
        // Arrange
        _userManagerMock.Setup(x => x.FindByIdAsync(_applicationUser.Id))
            .ReturnsAsync(_applicationUser);

        _cacheMock.Setup(x => x.GetOrSetAsync(
                It.IsAny<string>(),
                It.IsAny<Func<FusionCacheFactoryExecutionContext<User>, CancellationToken, Task<User>>>(),
                It.IsAny<MaybeValue<User>>(),
                It.IsAny<FusionCacheEntryOptions>(),
                It.IsAny<IEnumerable<string>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(_applicationUser);

        // Act
        var result = await _sut.GetUserByIdAsync(_applicationUser.Id);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Succeeded, Is.True);
            Assert.That(result.User, Is.EqualTo(_applicationUser));
        });
    }

    [Test]
    public async Task GetUserByIdAsync_WhenUserDoesNotExist_ReturnsFailedResult()
    {
        // Arrange
        _userManagerMock.Setup(x => x.FindByIdAsync(_applicationUser.Id))
            .ReturnsAsync((User)null!); // Simulate user not found

        // Act
        var result = await _sut.GetUserByIdAsync(_applicationUser.Id);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.Succeeded, Is.False);
            Assert.That(result.Errors, Is.Not.Empty);
        });
    }

    [Test]
    public async Task GetUserRolesAsync_WhenUserExists_ReturnsUserRoles()
    {
        // Arrange
        List<string> roles = ["Admin", "User"];
        _userManagerMock.Setup(x => x.FindByEmailAsync(_applicationUser.Email!))
            .ReturnsAsync(_applicationUser);
        _userManagerMock.Setup(x => x.GetRolesAsync(_applicationUser))
            .ReturnsAsync(roles);

        _cacheMock.Setup(x => x.GetOrSetAsync(
           It.IsAny<string>(),
           It.IsAny<Func<FusionCacheFactoryExecutionContext<List<string>>, CancellationToken, Task<List<string>>>>(),
           It.IsAny<MaybeValue<List<string>>>(),
           It.IsAny<FusionCacheEntryOptions>(),
           It.IsAny<IEnumerable<string>>(),
           It.IsAny<CancellationToken>()
       ))
       .ReturnsAsync(roles);

        // Act
        var result = await _sut.GetUserRolesAsync(_applicationUser.Email!);

        // Assert
        Assert.That(result, Is.EqualTo(roles));
    }

    [Test]
    public async Task GetAllUsersAsync_ReturnsListOfUsers()
    {
        // Arrange
        var users = new List<User> { _applicationUser };

        _userManagerMock.Setup(x => x.GetUsersInRoleAsync(RoleDefaults.User))
            .ReturnsAsync(users);

        _cacheMock.Setup(x => x.GetOrSetAsync(
            It.IsAny<string>(),
            It.IsAny<Func<FusionCacheFactoryExecutionContext<List<User>>, CancellationToken, Task<List<User>>>>(),
            It.IsAny<MaybeValue<List<User>>>(),
            It.IsAny<FusionCacheEntryOptions>(),
            It.IsAny<IEnumerable<string>>(),
            It.IsAny<CancellationToken>()
        ))
        .ReturnsAsync(users);

        // Act
        var result = await _sut.GetAllUsersAsync();

        // Assert
        Assert.That(result, Is.EquivalentTo(users));
    }
}