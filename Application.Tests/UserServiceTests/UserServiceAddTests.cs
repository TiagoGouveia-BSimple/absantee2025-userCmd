using Application.DTO;
using Application.IPublishers;
using Application.IService;
using Application.Services;
using AutoMapper;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Moq;
using Xunit;

public class UserServiceTests
{
    [Fact]
    public async Task WhenGivenGoodDTO_ThenReturnCorrectUserDTO()
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

        var createUserDto = new CreateUserDTO
        {
            Names = "John",
            Surnames = "Doe",
            Email = "john@example.com",
            FinalDate = period._finalDate
        };

        var expectedUserDto = new UserDTO
        {
            Id = userId,
            Names = "John",
            Surnames = "Doe",
            Email = "john@example.com",
            Period = period
        };

        userFactoryMock
            .Setup(f => f.Create("John", "Doe", "john@example.com", period._finalDate))
            .ReturnsAsync(userDomainMock.Object);


        var userService = new UserService(
            userRepositoryMock.Object,
            userFactoryMock.Object,
            publisherMock.Object
        );

        // Act
        var result = await userService.Add(createUserDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedUserDto.Id, result.Id);
        Assert.Equal(expectedUserDto.Names, result.Names);
        Assert.Equal(expectedUserDto.Surnames, result.Surnames);
        Assert.Equal(expectedUserDto.Email, result.Email);

        userRepositoryMock.Verify(r => r.AddAsync(userDomainMock.Object));
        userRepositoryMock.Verify(r => r.SaveChangesAsync());
        publisherMock.Verify(p => p.PublishCreatedUserMessageAsync(
            userDomainMock.Object.Id,
            userDomainMock.Object.Names,
            userDomainMock.Object.Surnames,
            userDomainMock.Object.Email,
            userDomainMock.Object.PeriodDateTime
        ));
    }
}
