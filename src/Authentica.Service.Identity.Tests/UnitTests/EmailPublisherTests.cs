﻿using Application.Contracts;
using MassTransit;
using Microsoft.FeatureManagement;

namespace Authentica.Service.Identity.Tests.UnitTests;

[TestFixture]
public class EmailPublisherTests
{
    [Test]
    public async Task PublishIsSuccessfulWithCorrectMessage()
    {
        // Arrange
        var serviceProviderMock = new ServiceProviderMock();
        var busMock = new Mock<IPublishEndpoint>();
        var featureManagerMock = new FeatureManagerMock();

        featureManagerMock.Setup(f => f.IsEnabledAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        serviceProviderMock.Setup(provider => provider.GetService(typeof(IPublishEndpoint)))
            .Returns(busMock.Object);

        serviceProviderMock.Setup(provider => provider.GetService(typeof(IFeatureManager)))
            .Returns(featureManagerMock.Object);

        EmailPublisher publisher = new(serviceProviderMock.Object);

        // Act
        await publisher.Publish(It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>());

        // Assert
        busMock.Verify(bus => bus.Publish(It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
