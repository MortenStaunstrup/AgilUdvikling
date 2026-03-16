using API.Controllers;
using API.Models;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UserAPI.Tests;

[TestClass]
public sealed class UserControllerTests
{
    
    private Mock<IUserRepository> _mockRepository;
    private UsersController _controller;
    
    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<IUserRepository>();
        _controller = new UsersController(_mockRepository.Object);
    }
    
    [TestMethod]
    public void Valid_Registration_Returns_200_Code_And_Adds_User()
    {
        // Arrange
        var user = new UserDTO()
        {
            Email = "Umut@hjemmeværnet.dk",
            Name = "Umut",
            Password = "Umut123"
        };
        _mockRepository.Setup(r => r.Register(user))
            .Returns(0);

        // Act
        var result = _controller.RegisterUser(user) as CreatedResult;

        // Assert
        Assert.IsInstanceOfType<CreatedResult>(result);
        _mockRepository.Verify(r => r.Register(user), Times.Once);

    }

    [TestMethod]
    public void Username_Already_Exists_Returns_400_Bad_Request()
    {
        // Arrange
        _mockRepository.Setup(r => r.Register(It.IsAny<UserDTO>()))
            .Returns(2);

        // Act
        var result = _controller.RegisterUser(new UserDTO()) as  BadRequestResult;

        // Assert
        Assert.IsInstanceOfType<BadRequestResult>(result);
        _mockRepository.Verify(r => r.Register(It.IsAny<UserDTO>()), Times.Once);
    }

    [TestMethod]
    public void NULL_DTO_Returns_400_Bad_Request()
    {
        // Arrange
        _mockRepository.Setup(r => r.Register(null))
            .Returns(1);
        
        // Act
        var result = _controller.RegisterUser(null) as BadRequestResult;
        
        // Assert
        Assert.IsInstanceOfType<BadRequestResult>(result);
    }

    [TestMethod]
    public void Empty_Username_Returns_400_Bad_Request()
    {
        // Arrange
        var userDTO = new UserDTO()
        {
            Email = "Somethinig@agag",
            Name = "",
            Password = "Erarararah"
        };
        _mockRepository.Setup(r => r.Register(userDTO))
            .Returns(1);
        
        // Act
        var result = _controller.RegisterUser(userDTO) as BadRequestResult;
        Assert.IsInstanceOfType<BadRequestResult>(result);
    }

    [TestMethod]
    public void Empty_Password_Returns_400_Bad_Request()
    {
        // Arrange
        var userDTO = new UserDTO()
        {
            Email = "Somethinig@agag",
            Name = "Mortey",
            Password = ""
        };
        _mockRepository.Setup(r => r.Register(userDTO))
            .Returns(1);
        
        // Act
        var result = _controller.RegisterUser(userDTO) as BadRequestResult;
        Assert.IsInstanceOfType<BadRequestResult>(result);
    }

    [TestMethod]
    public void WhiteSpace_Username_Returns_400_Bad_Request()
    {
        // Arrange
        var userDTO = new UserDTO()
        {
            Email = "Somethinig@agag",
            Name = "      ",
            Password = "Erarararah"
        };
        _mockRepository.Setup(r => r.Register(userDTO))
            .Returns(1);
        
        // Act
        var result = _controller.RegisterUser(userDTO) as BadRequestResult;
        Assert.IsInstanceOfType<BadRequestResult>(result);
    }

    [TestMethod]
    public void Repository_Throws_Exception_During_Add_Returns_500_Server_Error()
    {
        // Arrange
        _mockRepository.Setup(r => r.Register(It.IsAny<UserDTO>()))
            .Returns(-1);
        
        // Act
        var result = _controller.RegisterUser(new UserDTO()) as StatusCodeResult;
        Assert.IsInstanceOfType<StatusCodeResult>(result);
        var expected = 500;
        Assert.AreEqual(expected, result.StatusCode);
    }
    
}