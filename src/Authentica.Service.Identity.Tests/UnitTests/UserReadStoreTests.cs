using Api.Constants;
using Application.Stores;

namespace Authentica.Service.Identity.Tests.UnitTests;

public class UserReadStoreTests
{
    private Mock<UserManager<User>> _userManagerMock;
    private Mock<IServiceProvider> _serviceProviderMock;
    private UserReadStore _sut;
    private User _applicationUser;

    [SetUp]
    public void SetUp()
    {
        // Initialize the mock UserManager
        _userManagerMock = new UserManagerMock<User>().Mock();

        _serviceProviderMock = new Mock<IServiceProvider>();

        // Injecting the mocked service provider into UserReadStore
        _sut = new UserReadStore(_serviceProviderMock.Object);

        // Sample user for test cases
        _applicationUser = new User
        {
            Id = "user-id",
            Email = "user@example.com"
        };

        // Setup IServiceProvider to return the mocked UserManager
        _serviceProviderMock.Setup(x => x.GetService(typeof(UserManager<User>)))
            .Returns(_userManagerMock.Object);
    }

    #region GetUserByEmailAsync (ClaimsPrincipal)

    [Test]
    public async Task GetUserByEmailAsync_WhenUserExists_ReturnsSuccessResult()
    {
        // Arrange
        var emailClaim = new Claim(ClaimTypes.Email, _applicationUser.Email!);
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity([emailClaim]));

        _userManagerMock.Setup(x => x.FindByEmailAsync(_applicationUser.Email!))
            .ReturnsAsync(_applicationUser);

        // Act
        var result = await _sut.GetUserByEmailAsync(claimsPrincipal);

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
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[] { emailClaim }));

        _userManagerMock.Setup(x => x.FindByEmailAsync(_applicationUser.Email!))
            .ReturnsAsync((User)null!); // Simulate user not found

        // Act
        var result = await _sut.GetUserByEmailAsync(claimsPrincipal);

        // Assert
        Assert.That(result.Succeeded, Is.False);
        Assert.That(result.Errors, Is.Not.Empty);
    }

    #endregion

    #region GetUserByEmailAsync (string)

    #region GetUserByEmailAsync (string)

    [Test]
    public async Task GetUserByEmailAsync_String_WhenUserExists_ReturnsSuccessResult()
    {
        // Arrange
        _userManagerMock.Setup(x => x.FindByEmailAsync(_applicationUser.Email!))
            .ReturnsAsync(_applicationUser);

        // Act
        var result = await _sut.GetUserByEmailAsync(_applicationUser.Email!);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Succeeded, Is.True);
            Assert.That(result.User, Is.EqualTo(_applicationUser));
        });
    }

    [Test]
    public async Task GetUserByEmailAsync_String_WhenUserDoesNotExist_ReturnsFailedResult()
    {
        // Arrange
        _userManagerMock.Setup(x => x.FindByEmailAsync(_applicationUser.Email!))
            .ReturnsAsync((User)null!); // Simulate user not found

        // Act
        var result = await _sut.GetUserByEmailAsync(_applicationUser.Email!);

        // Assert
        Assert.That(result.Succeeded, Is.False);
        Assert.That(result.Errors, Is.Not.Empty);
    }

    #endregion

    #endregion

    #region GetUserByIdAsync

    [Test]
    public async Task GetUserByIdAsync_WhenUserExists_ReturnsSuccessResult()
    {
        // Arrange
        _userManagerMock.Setup(x => x.FindByIdAsync(_applicationUser.Id))
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

        // Assert
        Assert.That(result.Succeeded, Is.False);
        Assert.That(result.Errors, Is.Not.Empty);
    }

    #endregion

    #region GetUserRolesAsync

    [Test]
    public async Task GetUserRolesAsync_WhenUserExists_ReturnsUserRoles()
    {
        // Arrange
        var roles = new List<string> { "Admin", "User" };
        _userManagerMock.Setup(x => x.FindByEmailAsync(_applicationUser.Email!))
            .ReturnsAsync(_applicationUser);
        _userManagerMock.Setup(x => x.GetRolesAsync(_applicationUser))
            .ReturnsAsync(roles);

        // Act
        var result = await _sut.GetUserRolesAsync(_applicationUser.Email!);

        // Assert
        Assert.That(result, Is.EqualTo(roles));
    }

    #endregion

    #region GetAllUsersAsync

    [Test]
    public async Task GetAllUsersAsync_ReturnsListOfUsers()
    {
        // Arrange
        var users = new List<User> { _applicationUser };
        _userManagerMock.Setup(x => x.GetUsersInRoleAsync(RoleDefaults.User))
            .ReturnsAsync(users);

        // Act
        var result = await _sut.GetAllUsersAsync();

        // Assert
        Assert.That(result, Is.EqualTo(users));
    }

    #endregion
}