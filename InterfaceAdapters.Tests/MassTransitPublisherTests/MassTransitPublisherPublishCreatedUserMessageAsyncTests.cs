using Domain.Messages;
using Domain.Models;
using InterfaceAdapters.Publishers;
using MassTransit;
using Moq;
using Xunit;

namespace InterfaceAdapters.IntegrationTests.MassTransitPublisherTests;

public class MassTransitPublisherPublishCreatedUserMessageAsyncTests
{
    [Fact]
    public async Task WhenPublisherIsCalled_ThenPublishUser()
    {
        // Arrange
        var endpointMock = new Mock<IPublishEndpoint>();
        var publisher = new MassTransitPublisher(endpointMock.Object);

        var userId = Guid.NewGuid();

        // Act
        await publisher.PublishCreatedUserMessageAsync(userId, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<PeriodDateTime>());

        // Assert
        endpointMock.Verify(p => p.Publish(
            It.Is<UserCreatedMessage>(m =>
            m.Id == userId),
            It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
}
