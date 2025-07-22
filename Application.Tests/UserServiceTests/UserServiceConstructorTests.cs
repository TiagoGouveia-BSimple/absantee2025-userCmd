using Application.IPublishers;
using Application.Services;
using AutoMapper;
using Domain.Factory;
using Domain.IRepository;
using Moq;

namespace Application.Tests.UserServiceTests;

public class UserServiceConstructorTests
{
    [Fact]
    public void WhenReceivingGoodArguments_ThenObjectIsInstantiated()
    {
        // Arrange
        var repo = new Mock<IUserRepository>();
        var factory = new Mock<IUserFactory>();
        var publisher = new Mock<IMessagePublisher>();

        // Act
        new UserService(repo.Object, factory.Object, publisher.Object);
    
        // Assert
    }
}
