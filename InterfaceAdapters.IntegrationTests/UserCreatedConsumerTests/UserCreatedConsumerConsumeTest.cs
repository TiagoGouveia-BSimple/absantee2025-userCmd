using System.Threading.Tasks;
using Application.Services;
using Domain.Messages;
using Domain.Models;
using MassTransit;
using Moq;
using Xunit;

namespace InterfaceAdapters.IntegrationTests.UserCreatedConsumerTests;

public class UserCreatedConsumerConsumeTest
{
    [Fact]
    public async Task WhenMessageIsConsumed_ThenServiceMethodIsCalledWithData()
    {
        // Arrange
        var serviceMock = new Mock<IUserService>();
        var consumer = new UserCreatedConsumer(serviceMock.Object);

        var messageId = new Guid();
        var name = "Test";
        var surname = "Testinald";
        var email = "test@example.com";
        var period = It.IsAny<PeriodDateTime>();

        var message = new UserCreatedMessage(messageId, name, surname, email, period);

        var contextMock = Mock.Of<ConsumeContext<UserCreatedMessage>>(c => c.Message == message);

        // Act
        await consumer.Consume(contextMock);

        // Assert
        serviceMock.Verify(s => s.AddConsumed(messageId, name, surname, email, period));
    }
}
