using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Options;
using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Authentica.Service.Identity.Tests.UnitTests;

[TestFixture]
public class TwoFactorTotpProviderTests
{
    private Mock<IServiceProvider> _serviceProviderMock;
    private Mock<UserManager<User>> _userManagerMock;
    private Mock<IOptions<IdentityOptions>> _identityOptionsMock;
    private TwoFactorTotpProvider _provider;

    [SetUp]
    public void SetUp()
    {
        _userManagerMock = new Mock<UserManager<User>>(
            Mock.Of<IUserStore<User>>());

        _serviceProviderMock = new Mock<IServiceProvider>();
        _serviceProviderMock
            .Setup(sp => sp.GetService(typeof(UserManager<User>)))
            .Returns(_userManagerMock.Object);

        var identityOptions = new IdentityOptions();
        _identityOptionsMock = new Mock<IOptions<IdentityOptions>>();
        _identityOptionsMock.Setup(opt => opt.Value).Returns(identityOptions);

        _provider = new TwoFactorTotpProvider(_serviceProviderMock.Object, _identityOptionsMock.Object);
    }

    [Test]
    public async Task GenerateKeyAsync_Returns_New_Authenticator_Key()
    {
        // Arrange
        var user = new User();
        var expectedKey = "new-authenticator-key";
        _userManagerMock.Setup(um => um.GenerateNewAuthenticatorKey())
            .Returns(expectedKey);

        // Act
        var result = await _provider.GenerateKeyAsync(user);

        // Assert
        Assert.That(expectedKey, Is.EqualTo(result));
    }

    [Test]
    public async Task GenerateQrCodeUriAsync_Returns_Correct_QrCodeUri()
    {
        // Arrange
        var user = new User();
        var email = "user@example.com";
        var key = "new-authenticator-key";
        var unformattedKey = "unformatted-auth-key";
        var issuer = UrlEncoder.Default.Encode(IdentityConstants.TwoFactorUserIdScheme);

        _userManagerMock.Setup(um => um.GenerateNewAuthenticatorKey()).Returns(key);
        _userManagerMock.Setup(um => um.GetAuthenticatorKeyAsync(user))
            .ReturnsAsync(unformattedKey);
        _userManagerMock.Setup(um => um.GetEmailAsync(user)).ReturnsAsync(email);

        // Act
        var result = await _provider.GenerateQrCodeUriAsync(user);

        // Assert
        var expectedUri = $"otpauth://totp/{issuer}:{UrlEncoder.Default.Encode(email)}?secret={unformattedKey}&issuer={issuer}&digits=6";
        Assert.That(expectedUri, Is.EqualTo(result));
    }

    [Test]
    public void GenerateQrCodeUriAsync_Throws_When_AuthenticatorKey_Is_Null()
    {
        // Arrange
        var user = new User();
        _userManagerMock.Setup(um => um.GetAuthenticatorKeyAsync(user))
            .ReturnsAsync((string)null!); // No authenticator key

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() => _provider.GenerateQrCodeUriAsync(user));
    }

    [Test]
    public void FormatKey_Returns_Formatted_Key()
    {
        // Arrange
        var unformattedKey = "1234567890123456";

        // Act
        var result = _provider.FormatKey(unformattedKey);

        // Assert
        Assert.That(result, Is.EqualTo("1234 5678 9012 3456"));
    }

    [Test]
    public async Task ValidateAsync_Returns_True_When_Token_Is_Valid()
    {
        // Arrange
        var user = new User();
        var code = "123456";
        _userManagerMock.Setup(um => um.VerifyTwoFactorTokenAsync(user, _userManagerMock.Object.Options.Tokens.AuthenticatorTokenProvider, code))
            .ReturnsAsync(true);

        // Act
        var result = await _provider.ValidateAsync(code, user);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task ValidateAsync_Returns_False_When_Token_Is_Invalid()
    {
        // Arrange
        var user = new User();
        var code = "123456";
        _userManagerMock.Setup(um => um.VerifyTwoFactorTokenAsync(user, _userManagerMock.Object.Options.Tokens.AuthenticatorTokenProvider, code))
            .ReturnsAsync(false);

        // Act
        var result = await _provider.ValidateAsync(code, user);

        // Assert
        Assert.That(result, Is.False);
    }
}
