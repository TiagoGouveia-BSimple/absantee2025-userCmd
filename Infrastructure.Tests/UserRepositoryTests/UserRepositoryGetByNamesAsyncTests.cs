using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;
using Domain.Visitor;
using Infrastructure.DataModel;
using AutoMapper;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Domain.Factory;
using Infrastructure.Resolvers;

namespace Infrastructure.Tests.UserRepositoryTests;

public class UserRepositoryGetByNamesAsyncTests {
    [Theory]
    [InlineData("John", "Johnny", 2)]
    [InlineData("Johnny", "Pedro", 1)]
    [InlineData("Morgan", "Pedro", 0)]
    public async Task WhenGettingByNamesAsync_ShouldReturnCorrectUsers(string name1, string name2, int expectedCount)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AbsanteeContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new AbsanteeContext(options);

        var id1 = new Guid();
        var user1 = new UserDataModel
        {
            Id = id1,
            Names = name1,
            Surnames = "Doe",
            Email = "user1@gmail.com"
        };

        var id2 = new Guid();
        var user2 = new UserDataModel
        {
            Id = id2,
            Names = name2,
            Surnames = "Bravo",
            Email = "user2@gmail.com"
        };

        var userMock = new Mock<IUser>();
        userMock.Setup(u => u.Id).Returns(user1.Id);
        userMock.Setup(u => u.Names).Returns(user1.Names);

        context.Users.AddRange(user1, user2);
        await context.SaveChangesAsync();

        var userFactoryMock = new Mock<IUserFactory>();
        userFactoryMock.Setup(f => f.Create(user1)).Returns(
            new User(user1.Id, user1.Names, user1.Surnames, user1.Email, user1.PeriodDateTime));
        userFactoryMock.Setup(f => f.Create(user2)).Returns(
            new User(user2.Id, user2.Names, user2.Surnames, user2.Email, user2.PeriodDateTime));

        var converter = new UserDataModelConverter(userFactoryMock.Object);

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<IUser, UserDataModel>();
            cfg.CreateMap<UserDataModel, IUser>().ConvertUsing(converter);
        });
        var mapper = mockMapper.CreateMapper();

        var userRepository = new UserRepositoryEF(context, mapper);

        // Act
        var result = await userRepository.GetByNamesAsync("John");

        // Assert
        Assert.Equal(expectedCount, result.Count());
        var users = result.Select(id => userRepository.GetById(id));
        if (expectedCount > 0)
        {
            Assert.All(users, u => Assert.Contains("John", u.Names, StringComparison.OrdinalIgnoreCase));
        }
        else
        {
            Assert.Empty(result);
        }
    }
}
