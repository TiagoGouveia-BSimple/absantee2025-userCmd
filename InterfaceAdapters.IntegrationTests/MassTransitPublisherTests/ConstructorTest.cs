using InterfaceAdapters.Publishers;
using MassTransit;
using Moq;
using Xunit;

namespace InterfaceAdapters.IntegrationTests.MassTransitPublisherTests;

public class ConstructorTest
{
    [Fact]
    public void WhenConstructorIsCalled_ThenObjectIsInstantiated()
    {
        // Arrange
        var endpointMock = new Mock<IPublishEndpoint>();

        // Act
        var publisher = new MassTransitPublisher(endpointMock.Object);

        // Assert
    }
}
