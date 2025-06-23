using MassTransit;
using Moq;
using Xunit;

public class MassTransitPublisherPublishCreatedUserMessageAsyncTests
{
    [Fact]
    public void WhenUserIsCreated_ThenPublishUser()
    {
        // Arrange
        var endpointMock = new Mock<IPublishEndpoint>();

        // Act

        // Assert
    }
}