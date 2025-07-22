using Domain.Models;
using Infrastructure.DataModel;
using Moq;

namespace Infrastructure.Tests.UserDataModelTests;

public class ConstructorTest
{
    [Fact]
    public async Task WhenPassingGoodArguments_ThenObjectIsInstantiated()
    {
        // Arrange
        var user = new User("test", "test", "test@example.com", null);

        // Act
        var userDM = new UserDataModel(user);

        // Assert
        Assert.NotEqual(Guid.Empty, userDM.Id);
    }

    [Fact]
    public async Task WhenPassingGoodArgumentsAndNoId_ThenObjectIsInstantiatedWithNoId()
    {
        // Arrange
        var user = new User(Guid.Empty, "test", "test", "test@example.com", It.IsAny<PeriodDateTime>());

        // Act
        var userDM = new UserDataModel(user);

        // Assert
        Assert.Equal(Guid.Empty, userDM.Id);
    }
}
