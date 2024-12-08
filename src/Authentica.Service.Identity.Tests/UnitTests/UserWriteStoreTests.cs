using Application.Contracts;
using Application.Stores;

namespace Authentica.Service.Identity.Tests.UnitTests;

public class UserWriteStoreTests
{
    // System Under Test (SUT)
    private UserWriteStore _sut;

    // Dependencies
    private ServiceProviderMock _serviceProviderMock;
    private Mock<UserManager<User>> _userManagerMock;
    private UserReadStoreMock _userReadStoreMock;
    private IHttpContextAccessorMock _httpContextAccessorMock;
    private ApplicationReadStoreMock _applicationReadStoreMock;

    // Test Data
    private ClaimsPrincipal _testUser;
    private User _applicationUser;
    private RegisterRequest _registerRequest;

    [SetUp]
    public void Setup()
    {
        SetupDependencies();
        SetupTestData();
        CreateSystemUnderTest();
    }

    private void SetupDependencies()
    {
        // Initialize mock abstractions for dependencies
        _serviceProviderMock = new();
        _userManagerMock = new UserManagerMock<User>().Mock();
        _userReadStoreMock = new();
        _httpContextAccessorMock = new();
        _applicationReadStoreMock = new();

        // Setup the HttpContext within the accessor mock
        var httpContextMock = new HttpContextMock();
        _httpContextAccessorMock.Mock().Setup(a => a.HttpContext).Returns(httpContextMock.Mock().Object);

        _serviceProviderMock.Mock()
            .Setup(x => x.GetService(typeof(IUserReadStore)))
            .Returns(_userReadStoreMock.Mock().Object);

        _serviceProviderMock.Mock()
            .Setup(x => x.GetService(typeof(IHttpContextAccessor)))
            .Returns(_httpContextAccessorMock.Mock().Object);

        _serviceProviderMock.Mock()
            .Setup(x => x.GetService(typeof(IApplicationReadStore)))
            .Returns(_applicationReadStoreMock.Mock().Object);

    }

    private void SetupTestData()
    {
        _testUser = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim(ClaimTypes.Email, "test@example.com")
        ]));

        _applicationUser = new User
        {
            Id = Guid.NewGuid().ToString(),
            Email = "test@example.com",
        };

        _registerRequest = new RegisterRequest
        {
            Email = "test@example.com",
            Password = "Test@1234",
            PhoneNumber = "123-456-7890",
            Address = new()
        };

    }

    private void CreateSystemUnderTest()
    {
        // Inject the mocked IServiceProvider into the SUT
        _sut = new UserWriteStore(_serviceProviderMock.Mock().Object);
    }
    [Test]
    public async Task SoftDeleteUserAsync_WhenSuccessful_ReturnsSuccessResult()
    {
        // Arrange
        _userReadStoreMock
            .Setup(x => x.GetUserByEmailAsync(_testUser, It.IsAny<CancellationToken>()))
            .ReturnsAsync(UserStoreResult.Success(_applicationUser));

        _userManagerMock
            .Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(IdentityResult.Success);

        // Setup IServiceProvider to return the correct mocks
        _serviceProviderMock.Mock()
            .Setup(x => x.GetService(typeof(UserManager<User>)))
            .Returns(_userManagerMock.Object);

        // Act
        var result = await _sut.SoftDeleteUserAsync(_testUser);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Succeeded, Is.True);
            Assert.That(result.User, Is.Not.Null);
            Assert.That(result.User.EntityDeletionStatus.IsDeleted, Is.True);
            Assert.That(result.User.EntityDeletionStatus.DeletedBy, Is.EqualTo(_applicationUser.Id));
            Assert.That(result.User.EntityDeletionStatus.DeletedOnUtc, Is.InstanceOf<DateTime>());
            Assert.That(result.Errors, Is.Empty);
        });
    }

    [Test]
    public async Task SoftDeleteUserAsync_WhenUpdateFails_ReturnsFailureResult()
    {
        // Arrange
        _userReadStoreMock.Mock()
            .Setup(x => x.GetUserByEmailAsync(_testUser, It.IsAny<CancellationToken>()))
            .ReturnsAsync(UserStoreResult.Success(_applicationUser));

        _userManagerMock
            .Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(IdentityResult.Failed());

        // Setup IServiceProvider to return the correct mocks
        _serviceProviderMock.Mock()
            .Setup(x => x.GetService(typeof(UserManager<User>)))
            .Returns(_userManagerMock.Object);

        _serviceProviderMock.Mock()
            .Setup(x => x.GetService(typeof(IUserReadStore)))
            .Returns(_userReadStoreMock.Object);

        // Act
        var result = await _sut.SoftDeleteUserAsync(_testUser);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Succeeded, Is.False);
            Assert.That(result.User, Is.Null);
        });
    }

    [Test]
    public void SoftDeleteUserAsync_WhenUserNotFound_ThrowsException()
    {
        // Arrange
        _userReadStoreMock.Mock()
            .Setup(x => x.GetUserByEmailAsync(_testUser, It.IsAny<CancellationToken>()))
            .ReturnsAsync(UserStoreResult.Success(null!));


        // Act & Assert
        var ex = Assert.ThrowsAsync<NullReferenceException>(
            async () => await _sut.SoftDeleteUserAsync(_testUser));
    }

    [Test]
    public async Task SoftDeleteUserAsync_VerifiesUserManagerUpdateParameters()
    {
        // Arrange
        _userReadStoreMock.Mock()
            .Setup(x => x.GetUserByEmailAsync(_testUser, It.IsAny<CancellationToken>()))
            .ReturnsAsync(UserStoreResult.Success(_applicationUser));

        _userManagerMock
            .Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(IdentityResult.Success);

        // Setup IServiceProvider to return the correct mocks
        _serviceProviderMock.Mock()
            .Setup(x => x.GetService(typeof(UserManager<User>)))
            .Returns(_userManagerMock.Object);

        // Act
        await _sut.SoftDeleteUserAsync(_testUser);

        // Assert
        _userManagerMock.Verify(x => x.UpdateAsync(It.Is<User>(u =>
            u.Id == _applicationUser.Id &&
            u.EntityDeletionStatus.IsDeleted == true &&
            u.EntityDeletionStatus.DeletedBy == _applicationUser.Id)),
            Times.Once);
    }

    [Test]
    public async Task CreateUserAsync_WhenSuccessful_ReturnsSuccessResult()
    {
        // Arrange
        _userManagerMock
            .Setup(x => x.CreateAsync(It.IsAny<User>()))
            .ReturnsAsync(IdentityResult.Success);

        // Setup IServiceProvider to return the correct mocks
        _serviceProviderMock.Mock()
            .Setup(x => x.GetService(typeof(UserManager<User>)))
            .Returns(_userManagerMock.Object);

        // Act
        var result = await _sut.CreateUserAsync(_registerRequest, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Succeeded, Is.True);
            Assert.That(result.User, Is.Not.Null);
            Assert.That(result.User.Email, Is.EqualTo(_registerRequest.Email));
            Assert.That(result.Errors, Is.Empty);
        });
    }

    [Test]
    public async Task CreateUserAsync_WhenCreationFails_ReturnsFailureResult()
    {
        // Arrange
        _userManagerMock
            .Setup(x => x.CreateAsync(It.IsAny<User>()))
            .ReturnsAsync(IdentityResult.Failed());

        // Setup IServiceProvider to return the correct mocks
        _serviceProviderMock.Mock()
            .Setup(x => x.GetService(typeof(UserManager<User>)))
            .Returns(_userManagerMock.Object);

        // Act
        var result = await _sut.CreateUserAsync(_registerRequest, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Succeeded, Is.False);
            Assert.That(result.User, Is.Null);
        });
    }

    [Test]
    public async Task ConfirmEmailAsync_WhenTokenIsValid_ReturnsSuccessResult()
    {
        // Arrange
        var token = "valid-token";
        _userManagerMock
            .Setup(x => x.VerifyUserTokenAsync(
                It.IsAny<User>(),
                TokenOptions.DefaultEmailProvider,
                EmailTokenConstants.ConfirmEmail,
                token))
            .ReturnsAsync(true);

        _userManagerMock
            .Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(IdentityResult.Success);

        // Setup IServiceProvider to return the correct mocks
        _serviceProviderMock.Mock()
            .Setup(x => x.GetService(typeof(UserManager<User>)))
            .Returns(_userManagerMock.Object);

        // Act
        var result = await _sut.ConfirmEmailAsync(_applicationUser, token);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Succeeded, Is.True);
            Assert.That(_applicationUser.EmailConfirmed, Is.True);
            Assert.That(result.Errors, Is.Empty);
        });
    }

    [Test]
    public async Task ConfirmEmailAsync_WhenTokenIsInvalid_ReturnsFailureResult()
    {
        // Arrange
        var token = "invalid-token";
        _userManagerMock
            .Setup(x => x.VerifyUserTokenAsync(
                It.IsAny<User>(),
                TokenOptions.DefaultEmailProvider,
                EmailTokenConstants.ConfirmEmail,
                token))
            .ReturnsAsync(false);

        // Setup IServiceProvider to return the correct mocks
        _serviceProviderMock.Mock()
            .Setup(x => x.GetService(typeof(UserManager<User>)))
            .Returns(_userManagerMock.Object);

        // Act
        var result = await _sut.ConfirmEmailAsync(_applicationUser, token);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Succeeded, Is.False);
            Assert.That(_applicationUser.EmailConfirmed, Is.False);
        });
    }

    [Test]
    public void ConfirmEmailAsync_WhenTokenIsNullOrWhiteSpace_ThrowsArgumentException()
    {
        // Arrange
        string invalidToken = " ";

        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            await _sut.ConfirmEmailAsync(_applicationUser, invalidToken));

        Assert.That(ex.Message, Is.EqualTo("The value cannot be an empty string or composed entirely of whitespace. (Parameter 'token')"));
    }

    [Test]
    public async Task ResetPasswordAsync_WhenTokenIsValid_ReturnsSuccessResult()
    {
        // Arrange
        var token = "valid-token";
        var newPassword = "NewPassword@123";
        var hashedPassword = "hashed-password";

        // Mock IPasswordHasher
        var passwordHasherMock = new Mock<IPasswordHasher<User>>();
        passwordHasherMock
            .Setup(x => x.HashPassword(It.IsAny<User>(), newPassword))
            .Returns(hashedPassword);

        _userManagerMock
            .Setup(x => x.VerifyUserTokenAsync(
                It.IsAny<User>(),
                TokenOptions.DefaultEmailProvider,
                EmailTokenConstants.ResetPassword,
                token))
            .ReturnsAsync(true);

        _userManagerMock
            .Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(IdentityResult.Success);

        // Setup IServiceProvider to return the correct mocks
        _serviceProviderMock.Mock()
            .Setup(x => x.GetService(typeof(UserManager<User>)))
            .Returns(_userManagerMock.Object);

        _serviceProviderMock.Mock()
            .Setup(x => x.GetService(typeof(IPasswordHasher<User>)))
            .Returns(passwordHasherMock.Object);

        // Act
        var result = await _sut.ResetPasswordAsync(_applicationUser, token, newPassword);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Succeeded, Is.True);
            Assert.That(_applicationUser.PasswordHash, Is.EqualTo(hashedPassword));
            Assert.That(result.Errors, Is.Empty);
        });

    }

    [Test]
    public async Task ResetPasswordAsync_WhenTokenIsInvalid_ReturnsFailureResult()
    {
        // Arrange
        var token = "invalid-token";
        var newPassword = "NewPassword@123";

        var passwordHasherMock = new Mock<IPasswordHasher<User>>();
        passwordHasherMock
            .Setup(x => x.HashPassword(It.IsAny<User>(), newPassword))
            .Returns(newPassword);

        _userManagerMock
            .Setup(x => x.VerifyUserTokenAsync(
                It.IsAny<User>(),
                TokenOptions.DefaultEmailProvider,
                EmailTokenConstants.ResetPassword,
                token))
            .ReturnsAsync(false);

        _serviceProviderMock.Mock()
            .Setup(x => x.GetService(typeof(UserManager<User>)))
            .Returns(_userManagerMock.Object);

        _serviceProviderMock.Mock()
           .Setup(x => x.GetService(typeof(IPasswordHasher<User>)))
           .Returns(passwordHasherMock.Object);

        // Act
        var result = await _sut.ResetPasswordAsync(_applicationUser, token, newPassword);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Succeeded, Is.False);
            Assert.That(result.Errors, Has.One.Items); // Assuming you return one error for invalid token
        });
    }

    [Test]
    public void ResetPasswordAsync_WhenTokenIsNullOrWhiteSpace_ThrowsArgumentException()
    {
        // Arrange
        var invalidToken = " ";
        var newPassword = "NewPassword@123";

        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            await _sut.ResetPasswordAsync(_applicationUser, invalidToken, newPassword));

        Assert.That(ex.Message, Is.EqualTo("The value cannot be an empty string or composed entirely of whitespace. (Parameter 'token')"));
    }

    [Test]
    public async Task RedeemMultiFactorRecoveryCodeAsync_WhenRedemptionSucceeds_ReturnsSuccessResult()
    {
        // Arrange
        var recoveryCode = "valid-code";

        _userManagerMock
            .Setup(x => x.RedeemTwoFactorRecoveryCodeAsync(It.IsAny<User>(), recoveryCode))
            .ReturnsAsync(IdentityResult.Success);

        _serviceProviderMock.Mock()
            .Setup(x => x.GetService(typeof(UserManager<User>)))
            .Returns(_userManagerMock.Object);

        // Act
        var result = await _sut.RedeemMultiFactorRecoveryCodeAsync(_applicationUser, recoveryCode);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Succeeded, Is.True);
            Assert.That(result.Errors, Is.Empty);
        });
    }

    [Test]
    public async Task RedeemMultiFactorRecoveryCodeAsync_WhenRedemptionFails_ReturnsFailureResult()
    {
        // Arrange
        var recoveryCode = "invalid-code";

        _userManagerMock
            .Setup(x => x.RedeemTwoFactorRecoveryCodeAsync(It.IsAny<User>(), recoveryCode))
            .ReturnsAsync(IdentityResult.Failed());

        _serviceProviderMock.Mock()
            .Setup(x => x.GetService(typeof(UserManager<User>)))
            .Returns(_userManagerMock.Object);

        // Act
        var result = await _sut.RedeemMultiFactorRecoveryCodeAsync(_applicationUser, recoveryCode);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Succeeded, Is.False);
            Assert.That(result.Errors, Is.Empty);
        });
    }

    [Test]
    public async Task RedeemMultiFactorRecoveryCodeAsync_WhenExceptionIsThrown_ReturnsFailureResult()
    {
        // Arrange
        var recoveryCode = "valid-code";
        var exception = new InvalidOperationException("Test exception");

        _userManagerMock
            .Setup(x => x.RedeemTwoFactorRecoveryCodeAsync(It.IsAny<User>(), recoveryCode))
            .ThrowsAsync(exception);

        _serviceProviderMock.Mock()
            .Setup(x => x.GetService(typeof(UserManager<User>)))
            .Returns(_userManagerMock.Object);

        // Act
        var result = await _sut.RedeemMultiFactorRecoveryCodeAsync(_applicationUser, recoveryCode);

        // Assert
        Assert.That(result.Succeeded, Is.False);
    }

    [Test]
    public async Task UpdateEmailAsync_WhenTokenIsValid_UpdatesEmailAndReturnsSuccessResult()
    {
        // Arrange
        var token = "valid-token";
        var newEmail = "newemail@example.com";

        _userManagerMock
            .Setup(x => x.VerifyUserTokenAsync(
                It.IsAny<User>(),
                TokenOptions.DefaultEmailProvider,
                EmailTokenConstants.UpdateEmail,
                token))
            .ReturnsAsync(true);

        _userManagerMock
            .Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(IdentityResult.Success);

        _serviceProviderMock.Mock()
            .Setup(x => x.GetService(typeof(UserManager<User>)))
            .Returns(_userManagerMock.Object);

        // Act
        var result = await _sut.UpdateEmailAsync(_applicationUser, newEmail, token);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Succeeded, Is.True);
            Assert.That(result.Errors, Is.Empty);
            Assert.That(_applicationUser.Email, Is.EqualTo(newEmail));
            Assert.That(_applicationUser.NormalizedEmail, Is.EqualTo(newEmail.ToUpper()));
            Assert.That(_applicationUser.UserName, Is.EqualTo(newEmail));
        });
    }

    [Test]
    public async Task UpdateEmailAsync_WhenTokenIsInvalid_ReturnsFailureResult()
    {
        // Arrange
        var token = "invalid-token";
        var newEmail = "newemail@example.com";

        _userManagerMock
            .Setup(x => x.VerifyUserTokenAsync(
                It.IsAny<User>(),
                TokenOptions.DefaultEmailProvider,
                EmailTokenConstants.UpdateEmail,
                token))
            .ReturnsAsync(false);

        _serviceProviderMock.Mock()
            .Setup(x => x.GetService(typeof(UserManager<User>)))
            .Returns(_userManagerMock.Object);

        // Act
        var result = await _sut.UpdateEmailAsync(_applicationUser, newEmail, token);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Succeeded, Is.False);
            Assert.That(result.Errors, Is.Empty);
        });
    }

    [Test]
    public async Task UpdateEmailAsync_WhenExceptionIsThrown_ReturnsFailureResultWithException()
    {
        // Arrange
        var token = "valid-token";
        var newEmail = "newemail@example.com";
        var exception = new InvalidOperationException("Test exception");

        _userManagerMock
            .Setup(x => x.VerifyUserTokenAsync(
                It.IsAny<User>(),
                TokenOptions.DefaultEmailProvider,
                EmailTokenConstants.UpdateEmail,
                token))
            .ThrowsAsync(exception);

        _serviceProviderMock.Mock()
            .Setup(x => x.GetService(typeof(UserManager<User>)))
            .Returns(_userManagerMock.Object);

        // Act
        var result = await _sut.UpdateEmailAsync(_applicationUser, newEmail, token);

        Assert.That(result.Succeeded, Is.False);
    }

    [Test]
    public async Task UpdatePhoneNumberAsync_WhenTokenIsValid_UpdatesPhoneNumberAndReturnsSuccessResult()
    {
        // Arrange
        var token = "valid-token";
        var newPhoneNumber = "1234567890";

        _userManagerMock
            .Setup(x => x.VerifyUserTokenAsync(
                It.IsAny<User>(),
                TokenOptions.DefaultEmailProvider,
                EmailTokenConstants.UpdatePhoneNumber,
                token))
            .ReturnsAsync(true);

        _userManagerMock
            .Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(IdentityResult.Success);

        _serviceProviderMock.Mock()
            .Setup(x => x.GetService(typeof(UserManager<User>)))
            .Returns(_userManagerMock.Object);

        // Act
        var result = await _sut.UpdatePhoneNumberAsync(_applicationUser, newPhoneNumber, token);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Succeeded, Is.True);
            Assert.That(result.Errors, Is.Empty);
            Assert.That(_applicationUser.PhoneNumber, Is.EqualTo(newPhoneNumber));
        });
    }

    [Test]
    public async Task UpdatePhoneNumberAsync_WhenTokenIsInvalid_ReturnsFailureResult()
    {
        // Arrange
        var token = "invalid-token";
        var newPhoneNumber = "1234567890";

        _userManagerMock
            .Setup(x => x.VerifyUserTokenAsync(
                It.IsAny<User>(),
                TokenOptions.DefaultEmailProvider,
                EmailTokenConstants.UpdatePhoneNumber,
                token))
            .ReturnsAsync(false);

        _serviceProviderMock.Mock()
            .Setup(x => x.GetService(typeof(UserManager<User>)))
            .Returns(_userManagerMock.Object);

        // Act
        var result = await _sut.UpdatePhoneNumberAsync(_applicationUser, newPhoneNumber, token);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Succeeded, Is.False);
            Assert.That(result.Errors, Is.Empty);
        });
    }

    [Test]
    public async Task UpdatePhoneNumberAsync_WhenExceptionIsThrown_ReturnsFailureResultWithException()
    {
        // Arrange
        var token = "valid-token";
        var newPhoneNumber = "1234567890";
        var exception = new InvalidOperationException("Test exception");

        _userManagerMock
            .Setup(x => x.VerifyUserTokenAsync(
                It.IsAny<User>(),
                TokenOptions.DefaultEmailProvider,
                EmailTokenConstants.UpdatePhoneNumber,
                token))
            .ThrowsAsync(exception);

        _serviceProviderMock.Mock()
            .Setup(x => x.GetService(typeof(UserManager<User>)))
            .Returns(_userManagerMock.Object);

        // Act
        var result = await _sut.UpdatePhoneNumberAsync(_applicationUser, newPhoneNumber, token);

        // Assert
        Assert.That(result.Succeeded, Is.False);
    }
}