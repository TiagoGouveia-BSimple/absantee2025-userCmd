using Application.IService;
using InterfaceAdapters.Controllers;
using Moq;
using Xunit;

namespace InterfaceAdapters.Tests.UserControllerTests;

public class ConstructorTest
{
    [Fact]
    public void WhenConstructorIsCalled_ThenObjectIsInstantiated()
    {
        // Arrange
        var serviceMock = new Mock<IUserService>();

        // Act
        var controller = new UserController(serviceMock.Object);

        // Assert
    }
}
