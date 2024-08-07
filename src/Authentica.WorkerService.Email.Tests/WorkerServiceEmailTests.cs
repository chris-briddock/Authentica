using Authentica.Common;

namespace Authentica.WorkerService.Email.Tests;

[TestFixture]
public class WorkerTests
{
    private Mock<ILogger<Worker>> _loggerMock;
    private Mock<IServiceProvider> _serviceProvider;
    private Mock<ISmtpClient> _smtpClientMock;
    private Mock<IConfiguration> _configurationMock;
    private Worker _worker;

    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<Worker>>();
        _serviceProvider = new Mock<IServiceProvider>();
        _smtpClientMock = new Mock<ISmtpClient>();
        _configurationMock = new Mock<IConfiguration>();

        _smtpClientMock.Setup(x => x.SendMailAsync(It.IsAny<MailMessage>())).Returns(Task.CompletedTask);

        _configurationMock.SetupGet(c => c["Email:Server"]).Returns("localhost");
        _configurationMock.SetupGet(c => c["Email:Port"]).Returns("25");
        _configurationMock.SetupGet(c => c["Email:Credentials:EmailAddress"]).Returns("test@example.com");
        _configurationMock.SetupGet(c => c["Email:Credentials:Password"]).Returns("password");

        _serviceProvider.Setup(x => x.GetService(typeof(IConfiguration))!).Returns(_configurationMock.Object);
        _serviceProvider.Setup(x => x.GetService(typeof(ISmtpClient))!).Returns(_smtpClientMock.Object);
        _worker = new Worker(_loggerMock.Object, _serviceProvider.Object);
    }

    [Test]
    public async Task Consume_ConfirmEmail_SendsEmail()
    {
        // Arrange
        var emailMessage = new EmailMessage
        {
            Type = EmailPublisherConstants.ConfirmEmail,
            EmailAddress = "recipient@example.com",
            Code = "https://example.com/confirm"
        };

        var consumeContextMock = new Mock<ConsumeContext<EmailMessage>>();
        consumeContextMock.Setup(c => c.Message).Returns(emailMessage);

        // Act
        await _worker.Consume(consumeContextMock.Object);

        // Assert
        _smtpClientMock.Verify(
            smtp => smtp.SendMailAsync(It.Is<MailMessage>(msg =>
                msg.To.Contains(new MailAddress("recipient@example.com")) &&
                msg.Subject == "Please confirm your email address." &&
                msg.Body.Contains("Your confirmation email code is https://example.com/confirm")
            )),
            Times.Once);
    }

    [Test]
    public async Task Consume_TwoFactorToken_SendsEmail()
    {
        // Arrange
        var emailMessage = new EmailMessage
        {
            Type = EmailPublisherConstants.TwoFactor,
            EmailAddress = "recipient@example.com",
            Code = "123456"
        };

        var consumeContextMock = new Mock<ConsumeContext<EmailMessage>>();
        consumeContextMock.Setup(c => c.Message).Returns(emailMessage);

        // Act
        await _worker.Consume(consumeContextMock.Object);

        // Assert
        _smtpClientMock.Verify(
            smtp => smtp.SendMailAsync(It.Is<MailMessage>(msg =>
                msg.To.Contains(new MailAddress("recipient@example.com")) &&
                msg.Subject == "You requested a two-factor code" &&
                msg.Body.Contains("123456")
            )),
            Times.Once);
    }

    [Test]
    public async Task Consume_ResetPassword_SendsEmail()
    {
        // Arrange
        var emailMessage = new EmailMessage
        {
            Type = EmailPublisherConstants.ResetPassword,
            EmailAddress = "recipient@example.com",
            Code = "654321"
        };

        var consumeContextMock = new Mock<ConsumeContext<EmailMessage>>();
        consumeContextMock.Setup(c => c.Message).Returns(emailMessage);

        // Act
        await _worker.Consume(consumeContextMock.Object);

        // Assert
        _smtpClientMock.Verify(
            smtp => smtp.SendMailAsync(It.Is<MailMessage>(msg =>
                msg.To.Contains(new MailAddress("recipient@example.com")) &&
                msg.Subject == "Password Reset Request" &&
                msg.Body.Contains("654321")
            )),
            Times.Once);
    }

    [Test]
    public async Task Consume_UpdateEmail_SendsEmail()
    {
        // Arrange
        var emailMessage = new EmailMessage
        {
            Type = EmailPublisherConstants.UpdateEmail,
            EmailAddress = "recipient@example.com",
            Code = "updateemailcode"
        };

        var consumeContextMock = new Mock<ConsumeContext<EmailMessage>>();
        consumeContextMock.Setup(c => c.Message).Returns(emailMessage);

        // Act
        await _worker.Consume(consumeContextMock.Object);

        // Assert
        _smtpClientMock.Verify(
            smtp => smtp.SendMailAsync(It.Is<MailMessage>(msg =>
                msg.To.Contains(new MailAddress("recipient@example.com")) &&
                msg.Subject == "Update Your Email Address" &&
                msg.Body.Contains("Your email update code is updateemailcode")
            )),
            Times.Once);
    }

    [Test]
    public async Task Consume_UpdatePhoneNumber_SendsEmail()
    {
        // Arrange
        var emailMessage = new EmailMessage
        {
            Type = EmailPublisherConstants.UpdatePhoneNumber,
            EmailAddress = "recipient@example.com",
            Code = "updatephonecode"
        };

        var consumeContextMock = new Mock<ConsumeContext<EmailMessage>>();
        consumeContextMock.Setup(c => c.Message).Returns(emailMessage);

        // Act
        await _worker.Consume(consumeContextMock.Object);

        // Assert
        _smtpClientMock.Verify(
            smtp => smtp.SendMailAsync(It.Is<MailMessage>(msg =>
                msg.To.Contains(new MailAddress("recipient@example.com")) &&
                msg.Subject == "Update Your Phone Number" &&
                msg.Body.Contains("Your phone number update code is updatephonecode")
            )),
            Times.Once);
    }
}
