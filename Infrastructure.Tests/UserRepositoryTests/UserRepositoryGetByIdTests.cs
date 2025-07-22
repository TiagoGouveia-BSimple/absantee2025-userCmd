using System.Runtime.Serialization;
using AutoMapper;
using Domain.Factory;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Infrastructure.Resolvers;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Infrastructure.Tests.UserRepositoryTests;

public class UserRepositoryGetByIdTests
{
    [Theory]
    [InlineData("9660cb14-3f6d-465a-80e8-f7a3a554c2ac", true)]
    [InlineData("9660cb14-3f6d-465a-80e8-f7a3a554c2ad", false)]
    public async Task WhenGettingById_ShouldReturnCorrectUser(string stringId, bool expected)
    {
        // Arrange
        var id = Guid.Parse("9660cb14-3f6d-465a-80e8-f7a3a554c2ac");

        var options = new DbContextOptionsBuilder<AbsanteeContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new AbsanteeContext(options);

        var user = new UserDataModel
        {
            Id = id,
            Names = "user",
            Surnames = "user",
            Email = "user@example.com"
        };

        var userMock = new Mock<IUser>();
        userMock.Setup(u => u.Id).Returns(user.Id);

        context.Users.AddRange(user);
        await context.SaveChangesAsync();

        var userFactoryMock = new Mock<IUserFactory>();
        userFactoryMock.Setup(f => f.Create(user)).Returns(
            new User(user.Id, user.Names, user.Surnames, user.Email, user.PeriodDateTime));

        var converter = new UserDataModelConverter(userFactoryMock.Object);

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<IUser, UserDataModel>();
            cfg.CreateMap<UserDataModel, IUser>().ConvertUsing(converter);
        });
        var mapper = mockMapper.CreateMapper();

        var userRepository = new UserRepositoryEF(context, mapper);

        // Act
        var result = userRepository.GetById(Guid.Parse(stringId));

        // Assert
        Assert.Equal(expected, result != null);
    }
}
