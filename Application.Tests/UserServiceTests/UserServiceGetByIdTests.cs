using Application.DTO;
using Application.IPublishers;
using Application.Services;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Moq;

namespace Application.Tests.UserServiceTests;

public class UserServiceGetByIdTests
{
    [Fact]
    public async Task WhenMethodIsCalledWithValidId_ThenReturnUser()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var userFactoryMock = new Mock<IUserFactory>();
        var publisherMock = new Mock<IMessagePublisher>();

        var userId = Guid.NewGuid();
        var period = new PeriodDateTime(DateTime.UtcNow, DateTime.UtcNow.AddDays(1));

        var userDomainMock = new Mock<IUser>();
        userDomainMock.SetupGet(u => u.Id).Returns(userId);
        userDomainMock.SetupGet(u => u.Names).Returns("John");
        userDomainMock.SetupGet(u => u.Surnames).Returns("Doe");
        userDomainMock.SetupGet(u => u.Email).Returns("john@example.com");
        userDomainMock.SetupGet(u => u.PeriodDateTime).Returns(period);


        userRepositoryMock.Setup(r => r.GetByIdAsync(userId))
            .ReturnsAsync(userDomainMock.Object );

        var userService = new UserService(
            userRepositoryMock.Object,
            userFactoryMock.Object,
            publisherMock.Object
        );

        // Act
        var result = await userService.GetById(userId);

        // Assert
        Assert.Equal(userId, result.Id);
        Assert.Equal("John", result.Names);
        Assert.Equal("Doe", result.Surnames);
        Assert.Equal("john@example.com", result.Email);
        Assert.Equal(period, result.Period);
    }
}
