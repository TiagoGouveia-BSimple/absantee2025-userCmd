using Application.DTO;
using Application.IPublishers;
using Application.Services;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Moq;

namespace Application.Tests.UserServiceTests;

public class UserServiceAddTests
{
    [Fact]
    public async Task WhenMethodIsCalled_ThenReturnListOfUsers()
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


        userRepositoryMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<IUser> { userDomainMock.Object });


        var userService = new UserService(
            userRepositoryMock.Object,
            userFactoryMock.Object,
            publisherMock.Object
        );

        // Act
        var result = await userService.GetAll();

        // Assert
        Assert.Single(result);
        var resultUser = result.First();
        Assert.Equal(userId, resultUser.Id);
        Assert.Equal("John", resultUser.Names);
        Assert.Equal("Doe", resultUser.Surnames);
        Assert.Equal("john@example.com", resultUser.Email);
        Assert.Equal(period, resultUser.Period);
    }
}
