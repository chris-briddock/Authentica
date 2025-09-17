using Common.Constants;
using Common.Events;

namespace Authentica.Service.Identity.Tests.UnitTests;

public class EmailPublisherTests
{
    [Test]
    public async Task PublishIsSuccessfulWithCorrectMessage()
    {
        // Arrange
        var busMock = new Mock<IPublishEndpoint>();
        var featureManagerMock = new Mock<IFeatureManager>();
        var serviceProviderMock = new Mock<IServiceProvider>();

        var publishCalled = false;
        ApplicationCreated? publishedMessage = null;

        // Setup the mock to capture the call
        busMock.Setup(x => x.Publish(It.IsAny<object>(), It.IsAny<CancellationToken>()))
               .Callback<object, CancellationToken>((msg, ct) =>
               {
                   publishCalled = true;
                   if (msg is ApplicationCreated appCreated)
                   {
                       publishedMessage = appCreated;
                   }
               })
               .Returns(Task.CompletedTask);

        featureManagerMock
            .Setup(f => f.IsEnabledAsync(FeatureFlagConstants.RabbitMq))
            .ReturnsAsync(true);
        featureManagerMock
            .Setup(f => f.IsEnabledAsync(FeatureFlagConstants.AzServiceBus))
            .ReturnsAsync(false);

        serviceProviderMock
            .Setup(sp => sp.GetService(typeof(IPublishEndpoint)))
            .Returns(busMock.Object);

        serviceProviderMock
            .Setup(sp => sp.GetService(typeof(IFeatureManager)))
            .Returns(featureManagerMock.Object);

        var publisher = new EmailPublisher(serviceProviderMock.Object);

        var occurredOn = DateTime.UtcNow;
        var message = new ApplicationCreated("Test", occurredOn, "Test");

        // Act
        await publisher.PublishAsync(message, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(publishCalled, Is.True, "Publish method should have been called");
            Assert.That(publishedMessage, Is.Not.Null, "Published message should not be null");
            Assert.That(publishedMessage!.Name, Is.EqualTo("Test"), "Message name should match");
            Assert.That(publishedMessage.CreatedBy, Is.EqualTo("Test"), "Message CreatedBy should match");
        });
    }



    [Test]
    public async Task PublishAsync_DoesNotPublish_WhenFeatureFlagsDisabled()
    {
        // Arrange
        var serviceProviderMock = new ServiceProviderMock();
        var busMock = new Mock<IPublishEndpoint>();
        var featureManagerMock = new FeatureManagerMock();

        featureManagerMock.Setup(f => f.IsEnabledAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        serviceProviderMock.Setup(provider => provider.GetService(typeof(IPublishEndpoint)))
            .Returns(busMock.Object);

        serviceProviderMock.Setup(provider => provider.GetService(typeof(IFeatureManager)))
            .Returns(featureManagerMock.Object);

        EmailPublisher publisher = new(serviceProviderMock.Object);
        var testMessage = new EmailMessage();

        // Act
        await publisher.PublishAsync(testMessage, CancellationToken.None);

        // Assert
        busMock.Verify(bus => bus.Publish(It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public void PublishAsync_ThrowsArgumentNullException_WhenEventIsNull()
    {
        // Arrange
        var serviceProviderMock = new ServiceProviderMock();
        EmailPublisher publisher = new(serviceProviderMock.Object);

        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(() => 
            publisher.PublishAsync<EmailMessage>(null!, CancellationToken.None));
    }
}
